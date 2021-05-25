using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
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
        public async Task SavePatient(Employee employee)
        {
            Employee empl = await db.Employees.FirstOrDefaultAsync(p => p.IdEmployee == employee.IdEmployee);
            Employee newEmp = new Employee();
            newEmp.IdEmployee = employee.IdEmployee;
            newEmp.Surname = empl.Surname;
            newEmp.Name = employee.Name;
            newEmp.Lastname = employee.Lastname;
            newEmp.IdPosition = employee.IdPosition;
            if (empl.Password == null)
            {
                empl.Password = PasswordHash.GetHash(employee.Password);
            }
            else empl.Password = employee.Password;
            if (empl == null)
            {
                await db.Employees.AddRangeAsync(newEmp);
            }
            db.SaveChanges();
        }

        public static List<Employee> SelectEmployees()
        {
            return DB.Employees.Select(p => p).ToList();
        }
    }
}
