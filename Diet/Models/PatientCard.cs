using Diet.Models;
using System;
using System.Collections.Generic;

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
        public decimal? DailyCalories { get; set; }
        public decimal? DailyProtein { get; set; }
        public decimal? DailyFats { get; set; }
        public decimal? DailyCarbohydrates { get; set; }
        public bool Activ { get; set; }

        public DateTime? StartDiet { get; set; }
        public DateTime? FinishDiet { get; set; }
        public int? IdActivityLevels { get; set; }

        public virtual ActivityLevel IdActivityLevelsNavigation { get; set; }
        public virtual Employee IdEmployeeNavigation { get; set; }
        public virtual Patient IdPatientNavigation { get; set; }
        public virtual ICollection<Meal> Meals { get; set; }
    }
}
