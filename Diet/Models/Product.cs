using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

#nullable disable

namespace Diet.Models
{
    public partial class Product
    {
        public int IdProduct { get; set; }
        [RegularExpression(@"^[А-Яа-я ]+$"), Required(ErrorMessage = "Некорректные данные"), StringLength(15)]
        public string Product1 { get; set; }
        static DietDBContext db = new DietDBContext();
        public static List<Product> SelectProducts()
        {
            return db.Products.Select(p => p).ToList();
        }

        public static async Task<Product> SelectOneProd(int id)
        {
            Product product = await db.Products.FirstOrDefaultAsync(p => p.IdProduct == id);
            return product;
        }
        public static async Task<int> MaxIdProd()
        {
            int ID = await db.Products.MaxAsync(p => p.IdProduct) + 1;
            return ID;
        }
        public async Task SaveProd(Product product)
        {

            if(await db.Products.FirstOrDefaultAsync(p => p.Product1 == product.Product1) != null)
            {
                Product p = await db.Products.FirstOrDefaultAsync(p => p.IdProduct == product.IdProduct);
                Product newProd = new Product();
                newProd.IdProduct = product.IdProduct;
                newProd.Product1 = product.Product1;
                if (p == null)
                {
                    await db.Products.AddRangeAsync(newProd);
                }
                db.SaveChanges();
            }

        }
        public static async Task DelProd(int id)
        {
            Product p = await db.Products.FirstOrDefaultAsync(p => p.IdProduct == id);
            List <DishesProduct> dp = db.DishesProducts.Where(p => p.IdProduct == p.IdProduct).ToList();
            if (db == null)
            {
                db.Products.Remove(p);
                db.SaveChanges();
            }
        }
    }
}
