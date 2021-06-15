using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Diet.Models.ViewModels
{
    public class DiaryModel
    {
        public List<Menu> menu { get; set; }
        public List<List<Product>> productInDish { get; set; }
        public Meals meal { get; set; }



        public static async Task listMeal(int id)
        {
            List<DiaryModel> model = new List<DiaryModel>();
            DietDBContext db = new DietDBContext();
            var card = db.PatientCards.OrderBy(p => p.IdCard).Where(p => p.IdPatient == id).Select(p => p).Last();
            if (db.Menu.Where(p => p.IdCard == card.IdCard).Count() == 0)
            {
                var DiagPatient = db.PatientDiagnoses.Where(p => p.IdCard == card.IdCard).Select(p => p).ToList();
                List<DiagnosesDish> listDish = new List<DiagnosesDish>();
                List<Product> product = new List<Product>();
                List<Dish> dishes = new List<Dish>();
                foreach (var t in DiagPatient)
                {
                    var list = db.DiagnosesDishes.Where(p => p.IdDiagnosis == t.IdDiagnosis && p.Allowed == false).Select(p => p.IdProduct).ToList();
                    foreach (var i in list)
                    {
                        product.Add(await db.Products.FirstOrDefaultAsync(p => p.IdProduct == i));
                    }
                }
                product.Distinct();
                foreach (var item in product)
                {
                    var prodInDish =await db.DishesProducts.Where(p => p.IdProduct != item.IdProduct).Select(p=>p.IdDish).ToListAsync();
                    foreach(var y in prodInDish)
                    {
                        dishes.Add(await db.Dishes.FirstOrDefaultAsync(p => p.IdDish == y));
                    }
                }
                dishes.Distinct();
                var idPosit= 1;
                if (await db.Menu.CountAsync() != 0)
                {
                   idPosit=await db.Menu.MaxAsync(p => p.IdPosition) + 1;
                }
                foreach(var d in dishes)
                {
                    db = new DietDBContext();
                    db.Menu.Add(new Menu { IdPosition = idPosit, Calories = d.Calories, Carbohydrates = d.Carbohydrates, Dish = d.Dish1, Fats = d.Fats, IdCard = card.IdCard, Protein = d.Protein });
                    db.SaveChanges();
                }
            }
            //bool 
            //if (db.Meals.Where(p =>p.IdPosition==db.Menu.Contains() p.Date < DateTime.Now).Count()== 0)
            //{
            //    List<Menu> menu = db.Menu.Where(p => p.IdCard == card.IdCard).ToList();

            //}
            //else
            //{
                
            //}

        }


    }
}
