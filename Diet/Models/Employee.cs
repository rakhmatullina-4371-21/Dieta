using System;
using System.Collections.Generic;

#nullable disable

namespace Diet.Models
{
    public partial class Employee
    {
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
