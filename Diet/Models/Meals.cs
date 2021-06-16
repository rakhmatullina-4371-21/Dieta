using System;
using System.Collections.Generic;

#nullable disable

namespace Diet.Models
{
    public partial class Meal
    {

        public int IdMeal { get; set; }
        public DateTime Date { get; set; }
        public int? IdMenu { get; set; }
        public virtual Menu IdMenuNavigation { get; set; }

    }
}
