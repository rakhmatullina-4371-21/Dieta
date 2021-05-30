using Microsoft.AspNetCore.Mvc.Rendering;
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

        public static IQueryable<SelectListItem> SelectLevelsAct()
        {
            var r= db.ActivityLevels.Select(r => new SelectListItem
            {
                Text = r.ActivityLevels,
                Value = r.IdActivityLevels.ToString()
            });
            return r;
        }
    }
}
