using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Diet.Models.ViewModels;

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
            return db.Products.Select(p => p).OrderBy(p=>p.Product1).ToList();
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
        public static async Task SaveProd(AddProductModel product)
        {


                Product p = await db.Products.FirstOrDefaultAsync(p => p.IdProduct == product.IdProduct);
                Product newProd = new Product();
                newProd.IdProduct = product.IdProduct;
                newProd.Product1 = product.Product1;
                List<DiagnosesDish> list = new List<DiagnosesDish>();
                int id = 1;
                if (db.DiagnosesDishes.Count() != 0)
                {
                    id = db.DiagnosesDishes.Max(p => p.id);
                }
                if (p == null)
                {
                    await db.Products.AddRangeAsync(newProd);
                } else
                {
                    db.SaveChanges();
                db = new DietDBContext(); 
                    db.Products.Update(newProd);
                }
                 await db.SaveChangesAsync();
                 var diag = await db.Diagnoses.Select(p=>p).OrderBy(p=>p.NameDiagnosis).ToListAsync();
                for(int i = 0; i < product.diagProd.Count(); i++)
                {
                    if (product.diagProd[i] == true)
                    {
                        list.Add(new DiagnosesDish {id=id, IdDiagnosis = diag.ElementAt(i).IdDiagnosis, IdProduct = product.IdProduct });
                    }
                    id++;
                }
                await  db.DiagnosesDishes.AddRangeAsync(list);
                db.SaveChanges();
            

        }
        public static async Task DelProd(int id)
        {
            Product p = await db.Products.FirstOrDefaultAsync(p => p.IdProduct == id);
             if(db.DishesProducts.Where(p => p.IdProduct == p.IdProduct).Count()!=0)
            {
                List<DishesProduct> dp = await db.DishesProducts.Where(p => p.IdProduct == p.IdProduct).ToListAsync();
                db.DishesProducts.RemoveRange(dp);
                db.Products.Remove(p);
            }
            if (db.DiagnosesDishes.Where(p => p.IdProduct == p.IdProduct).Count() != 0)
            {
                List<DiagnosesDish> dd = await db.DiagnosesDishes.Where(p => p.IdProduct == p.IdProduct).ToListAsync();
                db.DiagnosesDishes.RemoveRange(dd);
                db.Products.Remove(p);
            }
            db.Products.Remove(p);
            await db.SaveChangesAsync();
        }
    }
}
