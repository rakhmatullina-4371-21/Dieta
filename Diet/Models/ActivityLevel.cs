using System;
using System.Collections.Generic;

#nullable disable

namespace Diet.Models
{
    public partial class ActivityLevel
    {
        public ActivityLevel()
        {
            PatientCards = new HashSet<PatientCard>();
        }

        public int IdActivityLevels { get; set; }
        public string ActivityLevels { get; set; }
        public decimal Value { get; set; }
        public string Description { get; set; }

        public virtual ICollection<PatientCard> PatientCards { get; set; }
    }
}
