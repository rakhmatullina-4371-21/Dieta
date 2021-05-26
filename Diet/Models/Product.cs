using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable

namespace Diet.Models
{
    public partial class Product
    {
        public int IdProduct { get; set; }
        public string Product1 { get; set; }
        static DietDBContext db = new DietDBContext();
        public static List<Product> SelectDishes()
        {
            return db.Products.Select(p => p).ToList();
        }
    }
}
