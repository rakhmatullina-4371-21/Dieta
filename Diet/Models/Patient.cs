using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Linq;

#nullable disable

namespace Diet.Models
{
    public partial class Patient
    {
        public Patient()
        {
            PatientCards = new HashSet<PatientCard>();
        }
        static DietDBContext db = new DietDBContext();
        public int IdPatient { get; set; }

        [RegularExpression(@"^[А-Я]+[а-яА]*$"), Required(ErrorMessage ="Некорректные данные"), StringLength(15)]
        public string Surname { get; set; }

        [RegularExpression(@"^[А-Я]+[а-яА]*$"), Required(ErrorMessage = "Некорректные данные"), StringLength(15)]
        public string Name { get; set; }

        [RegularExpression(@"^[А-Я]+[а-яА]*$", ErrorMessage = "Некорректные данные"), StringLength(15)]
        public string Lastname { get; set; }

        [Required(ErrorMessage = "Не указан Email")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Не указан пароль")]
        public string Password { get; set; }
        public bool Woman { get; set; }

        [Display(Name = "DateOfBirth")]
        [DataType(DataType.Date)]
        [Range(typeof(DateTime), "1/1/1950", "1/1/2004", ErrorMessage = "Неверная дата рождения")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? DateOfBirth { get; set; }

        public virtual ICollection<PatientCard> PatientCards { get; set; }


        public static async Task<Patient> SelectPatient(int id)
        {
            Patient patient = await db.Patients.FirstOrDefaultAsync(p => p.IdPatient == id);
            return patient;
        }
        public static async Task<int> MaxIdPatient()
        {
            int ID = await db.Patients.MaxAsync(p => p.IdPatient) + 1;
            return ID;
        }





        public static async Task<Patient> SelectPatient(int? id)
        {
            Patient patient = await db.Patients.FirstOrDefaultAsync(p => p.IdPatient == id);
            return patient;
        }

        public async Task SavePatient(Patient patient)
        {
            Patient patt = await db.Patients.FirstOrDefaultAsync(p => p.IdPatient == patient.IdPatient);
            Patient pat = new Patient();
            pat.IdPatient=patient.IdPatient;
            pat.Surname = patient.Surname;
            pat.Name = patient.Name;
            pat.Lastname = patient.Lastname;
            pat.Woman = patient.Woman;

            pat.DateOfBirth = patient.DateOfBirth;
            pat.Login = patient.Login;
            if (pat.Password == null)
            {
                pat.Password = PasswordHash.GetHash(patient.Password);
            }
            else pat.Password = patient.Password;
            if (patt == null)
            {
                await db.Patients.AddRangeAsync(pat);
            }
            else
            {
                db.Patients.Update(pat);
            }
            db.SaveChanges();
        }

        public static  List<Patient> SelectPatients()
        {
            return  db.Patients.Select(p =>p).ToList();
        }
        public static List<Patient> SelectPatients( int id)
        {
            List<Patient> patients = new List<Patient>();
            var ListPatienNutr= db.PatientCards.Where(p=>p.IdEmployee==id).Select(p => p.IdPatient).ToList();
            foreach(var item in ListPatienNutr)
            {
                patients.Add(db.Patients.FirstOrDefault(p => p.IdPatient == item));
            }
            return patients;
        }
        public static List<Patient> SelectPatientCardNull(int idEmp)
        {
            List<Patient> patients = new List<Patient>();
            patients = db.Patients.Select(p => p).ToList();
            var pat =  (from t in db.Patients
                      join r in db.PatientCards on t.IdPatient equals r.IdPatient
                      where  r.IdEmployee==idEmp
                      select t).ToList();
            
            return patients.Except(pat).ToList();

        }

        public static async Task<bool> LoginContains( string login)
        {
            if (await db.Patients.FirstOrDefaultAsync(p => p.Login == login) != null || await db.Employees.FirstOrDefaultAsync(p => p.Login == login) != null)
            {
                return  false;
            }
            else return true;
        }
    }



}
