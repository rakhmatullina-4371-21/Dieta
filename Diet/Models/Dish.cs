using System;
using System.Collections.Generic;

#nullable disable

namespace Diet.Models
{
    public partial class Dish
    {
        public int IdDish { get; set; }
        public string Dish1 { get; set; }
        public decimal Calories { get; set; }
        public decimal Protein { get; set; }
        public decimal Fats { get; set; }
        public decimal Carbohydrates { get; set; }
    }
}
