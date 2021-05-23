using System;
using System.Collections.Generic;

#nullable disable

namespace Diet.Models
{
    public partial class DishesProduct
    {
        public int? IdDish { get; set; }
        public int? IdProduct { get; set; }

        public virtual Dish IdDishNavigation { get; set; }
        public virtual Product IdProductNavigation { get; set; }
    }
}
