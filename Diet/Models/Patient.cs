using System;
using System.Collections.Generic;

#nullable disable

namespace Diet.Models
{
    public partial class Patient
    {
        public Patient()
        {
            PatientCards = new HashSet<PatientCard>();
        }

        public int IdPatient { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public virtual ICollection<PatientCard> PatientCards { get; set; }
    }
}
