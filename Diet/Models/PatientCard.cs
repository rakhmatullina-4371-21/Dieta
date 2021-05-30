using Diet.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

#nullable disable

namespace Diet.Models
{
    public partial class PatientCard
    {
        public PatientCard()
        {
            Meals = new HashSet<Meal>();
        }

        public int IdCard { get; set; }
        public int? IdPatient { get; set; }
        public int? IdEmployee { get; set; }
        [RegularExpression(@"[0-9]*\,?[0-9][0-9]", ErrorMessage = "Некорректные данные")]
        public decimal? DailyCalories { get; set; }
        [RegularExpression(@"[0-9]*\,?[0-9][0-9]", ErrorMessage = "Некорректные данные")]
        public decimal? DailyProtein { get; set; }
        [RegularExpression(@"[0-9]*\,?[0-9][0-9]", ErrorMessage = "Некорректные данные")]
        public decimal? DailyFats { get; set; }
        [RegularExpression(@"[0-9]*\,?[0-9][0-9]", ErrorMessage = "Некорректные данные")]
        public decimal? DailyCarbohydrates { get; set; }
        public bool Activ { get; set; }

        [Display(Name = "Start")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? StartDiet { get; set; }

        [Display(Name = "Fihish")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]

        public DateTime? FinishDiet { get; set; }
        public int? IdActivityLevels { get; set; }

        public virtual ActivityLevel IdActivityLevelsNavigation { get; set; }
        public virtual Employee IdEmployeeNavigation { get; set; }
        public virtual Patient IdPatientNavigation { get; set; }
        public virtual ICollection<Meal> Meals { get; set; }


        static DietDBContext db = new DietDBContext();
        public static async Task SavePatCard(int idEmployee, PatientCard patientCard)
        {
            PatientCard card = new PatientCard();
            var listCard = db.PatientCards.Where(p => p.IdPatient == patientCard.IdPatient && p.IdEmployee==idEmployee).Select(p=>p);
            var countCards = await db.PatientCards.CountAsync();
            card.IdPatientNavigation = await db.Patients.FirstOrDefaultAsync(p => p.IdPatient == patientCard.IdPatient);
            card.IdPatient = patientCard.IdPatient;
            card.IdEmployeeNavigation = await db.Employees.FirstOrDefaultAsync(p => p.IdEmployee == idEmployee);
            card.IdEmployee = idEmployee;
            card.IdActivityLevelsNavigation = await db.ActivityLevels.FirstOrDefaultAsync(p => p.IdActivityLevels == 1);
            card.IdActivityLevels = patientCard.IdActivityLevels;
                if(countCards == 0)
                {
                    card.IdCard = 1;
                }
                else if(patientCard.IdCard==0)
                {
                    card.IdCard = await db.PatientCards.MaxAsync(p => p.IdCard) + 1;
                    card.Activ = true;
                }
                else {
                    card.IdCard = patientCard.IdCard;
                }

            db.PatientCards.Update(card);
                
            await db.SaveChangesAsync();
        }

        public static List<Patient> SelectPatientsNutr( int idEmp)
        {
            var pat = (from t in db.Patients
                       join r in db.PatientCards on t.IdPatient equals r.IdPatient
                       where r.IdEmployee == idEmp
                       select t).ToList();
            return pat;
        }
    }
}
