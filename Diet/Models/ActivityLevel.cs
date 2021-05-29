using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        static DietDBContext db = new DietDBContext();
        public static List<ActivityLevel> SelectLevelsAct()
        {
            return db.ActivityLevels.Select(p => p).ToList();
        }
    }
}
