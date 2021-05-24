using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable disable

namespace Diet.Models
{
    public partial class Employee
    {
        DietDBContext db = new DietDBContext();
        Employee employee;
        public  async Task<Employee> ReturnEmp(string id) 
        {
            employee = await db.Employees.FirstOrDefaultAsync(u => u.IdEmployee == int.Parse(id));
            return employee;
        }
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
        public string Phone { get; set; }

        public virtual Position IdPositionNavigation { get; set; }
        public virtual ICollection<PatientCard> PatientCards { get; set; }
    }
}
