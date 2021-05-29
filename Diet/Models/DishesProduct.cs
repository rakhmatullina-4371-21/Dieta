using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable disable

namespace Diet.Models
{
    public partial class DishesProduct
    {
        public int id { get; set; }
        public int? IdDish { get; set; }
        public int? IdProduct { get; set; }

        public virtual Dish IdDishNavigation { get; set; }
        public virtual Product IdProductNavigation { get; set; }

        static DietDBContext db = new DietDBContext();

        public static  List<int> SelectDishProd(int id)
        {
            List<int> list = new List<int>();
            foreach(var t in db.DishesProducts.Where(p => p.IdDish == id).Select(p => p.IdProduct).ToList())
            {
                list.Add(int.Parse(t.ToString()));
            }
            return list;
        }



       public  static async Task AddDisProd(Dictionary<int, int> disProd)
        {
            foreach( var t in disProd)
            {
                DishesProduct dishesProduct = new DishesProduct();
                dishesProduct.IdDish = t.Key;
                dishesProduct.IdProduct = t.Value;
                await db.DishesProducts.AddAsync(dishesProduct);
            }

            await db.SaveChangesAsync();
        }
    }
}
