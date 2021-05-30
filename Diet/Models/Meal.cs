using System;
using System.Collections.Generic;

#nullable disable

namespace Diet.Models
{
    public partial class Meal
    {
        public int IdMeals { get; set; }
        public DateTime Date { get; set; }
        public int? IdCard { get; set; }
        public string Dish { get; set; }
        public decimal? Calories { get; set; }

        public decimal? Protein { get; set; }
        public decimal? Fats { get; set; }
        public decimal? Carbohydrates { get; set; }

        public virtual PatientCard IdCardNavigation { get; set; }
    }
}
