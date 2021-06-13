using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

#nullable disable

namespace Diet.Models
{
    public partial class Employee
    {
        DietDBContext db = new DietDBContext();
        Employee employee;

        public Employee()
        {
            PatientCards = new HashSet<PatientCard>();
        }

        public int IdEmployee { get; set; }

        [RegularExpression(@"^[А-Я]+[а-яА]*$"), Required(ErrorMessage = "Некорректные данные"), StringLength(15)]
        public string Surname { get; set; }

        [RegularExpression(@"^[А-Я]+[а-яА]*$"), Required(ErrorMessage = "Некорректные данные"), StringLength(15)]
        public string Name { get; set; }

        [RegularExpression(@"^[А-Я]+[а-яА]*$"), StringLength(15)]
        public string Lastname { get; set; }

        [Required(ErrorMessage = "Не указан Email")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Не указан пароль")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Выберите должность")]
        public int? IdPosition { get; set; }

        public virtual Position IdPositionNavigation { get; set; }
        public virtual ICollection<PatientCard> PatientCards { get; set; }

        
        public async Task<Employee> ReturnEmp(string id)
        {
            employee = await db.Employees.FirstOrDefaultAsync(u => u.IdEmployee == int.Parse(id));
            return employee;
        }
        static DietDBContext DB = new DietDBContext();
        public static async Task<Employee> SelectEmployee(int id)
        {
            Employee employee = await DB.Employees.FirstOrDefaultAsync(p => p.IdEmployee == id);
            return employee;
        }
        public static async Task<int> MaxIdEmployee()
        {
            int ID = await DB.Employees.MaxAsync(p => p.IdEmployee) + 1;
            return ID;
        }
        public async Task SaveEmployee(Employee employee)
        {
            Employee empl = await db.Employees.FirstOrDefaultAsync(p => p.IdEmployee == employee.IdEmployee);
            Employee newEmp = new Employee();
            newEmp.IdEmployee = employee.IdEmployee;
            newEmp.Surname = employee.Surname;
            newEmp.Name = employee.Name;
            newEmp.Lastname = employee.Lastname;
            newEmp.IdPosition = employee.IdPosition;
            newEmp.Login = employee.Login;

            if (empl == null)
            {
                newEmp.Password = PasswordHash.GetHash(employee.Password);
            }
            else newEmp.Password = employee.Password;
            if (empl == null)
            {
                await db.Employees.AddRangeAsync(newEmp);
            }
            else
            {
                db.SaveChanges();
                db = new DietDBContext();
                 db.Employees.Update(newEmp);
            }
            db.SaveChanges();
        }

        public async Task DeleteEmployee(Employee employee)
        {
            Employee empl = await db.Employees.FirstOrDefaultAsync(p => p.IdEmployee == employee.IdEmployee);
            if (db.PatientCards.Where(p => p.IdEmployee == employee.IdEmployee).Count() != 0 && db.Employees.Count()!=0)
            {
                var card = await db.PatientCards.Where(p => p.IdEmployee == employee.IdEmployee).ToListAsync();
                foreach(var t in card)
                {
                    t.IdEmployee = Convert.ToInt32(db.Employees.OrderBy(p => p.IdEmployee).Select(p=>p.IdEmployee).FirstAsync());
                    db.Employees.Remove(empl);
                }
            }else if (db.Employees.Count() != 0)
            {
                db.Employees.Remove(empl);
            }
            db.SaveChanges();
        }
        public static List<Employee> SelectEmployees(string id)
        {
            return DB.Employees.Where(p=>p.IdEmployee!=int.Parse(id)).Select(p=>p).ToList();
        }


        public static string PositionEmp(int? id)
        {
            return DB.Positions.First(p => p.IdPosition == id).Position1;
        }

        public static List<Position> SelectPosition()
        {
            return DB.Positions.Select(p => p).ToList();
        }
    }

}
