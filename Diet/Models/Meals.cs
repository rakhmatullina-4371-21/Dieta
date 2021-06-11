using System;
using System.Collections.Generic;

#nullable disable

namespace Diet.Models
{
    public partial class Meals
    {

        public int IdMeals { get; set; }
        public DateTime Date { get; set; }
        public int? IdPosition { get; set; }
        public virtual Menu IdPositionNavigation { get; set; }

    }
}
