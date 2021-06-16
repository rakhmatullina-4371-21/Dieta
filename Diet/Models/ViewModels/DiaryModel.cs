using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Diet.Models.ViewModels
{
    public class DiaryModel
    {
     
        public List<Product> productInDish { get; set; }
        public Dish Dishes { get; set; }
        public Meal meal { get; set; }



        public static async Task<List<DiaryModel>> listMeal(int id)
        {
            List<DiaryModel> model = new List<DiaryModel>();
            DietDBContext db = new DietDBContext();
            List<Meal> meals = new List<Meal>();
            if (db.PatientCards.Where(p => p.IdPatient == id).Count() != 0)
            {
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
                        var prodInDish = await db.DishesProducts.Where(p => p.IdProduct != item.IdProduct).Select(p => p.IdDish).ToListAsync();
                        foreach (var y in prodInDish)
                        {
                            var t = await db.Dishes.FirstOrDefaultAsync(p => p.IdDish == y);
                            if (!dishes.Contains(t))
                            {
                                dishes.Add(t);
                            }

                        }
                    }
                    dishes.Distinct().OrderBy(p => p.Calories);
                    var idPosit = 1;
                    if (await db.Menu.CountAsync() != 0)
                    {
                        idPosit = await db.Menu.MaxAsync(p => p.IdMenu) + 1;
                    }
                    foreach (var d in dishes)
                    {
                        db = new DietDBContext();
                        db.Menu.Add(new Menu { IdMenu = idPosit, IdCard = card.IdCard, IdDish = d.IdDish });
                        idPosit++;
                        db.SaveChanges();
                    }
                }
                List<Menu> menus = db.Menu.Where(p => p.IdCard == card.IdCard).ToList();
                foreach (var t in menus)
                {
                    if (db.Meals.Where(p => p.IdMenu == t.IdMenu && p.Date >= DateTime.Now).Count() != 0)
                    {
                        meals.Add(db.Meals.First(p => p.IdMenu == t.IdMenu && p.Date >= DateTime.Now));
                    }
                }
                if (meals.Count == 0)
                {
                    List<Menu> menu = await db.Menu.Where(p => p.IdCard == card.IdCard).OrderByDescending(p => p.IdDish).ToListAsync();
                    List<Dish> dishesInMenu = new List<Dish>();
                    foreach (var i in menu)
                    {
                        dishesInMenu.Add(await db.Dishes.FirstOrDefaultAsync(p => p.IdDish == i.IdDish));
                    }
                    dishesInMenu.OrderBy(p => p.Calories);
                    var calories = Convert.ToInt32(card.DailyCalories / card.MealCount);
                    var fats = Convert.ToInt32(card.DailyFats / card.MealCount);
                    var carbohydrates = Convert.ToInt32(card.DailyCarbohydrates / card.MealCount);
                    var protein = Convert.ToInt32(card.DailyProtein / card.MealCount);
                    List<Dish> ListDish = new List<Dish>();
                    for (int i = 0; i < calories; i++)
                    {
                        if (await db.Dishes.Where(p => p.Calories == i).CountAsync() != 0)
                        {
                            ListDish.AddRange(await db.Dishes.Where(p => p.Calories == i).ToListAsync());
                        }
                    }

                    DateTime date = DateTime.Now;
                    Random rnd = new Random();
                    var iddish = rnd.Next(0, ListDish.Count());
                    int idMeal = 1;
                    if (db.Meals.Count() != 0)
                    {
                        idMeal = db.Meals.Max(p => p.IdMeal) + 1;
                    }
                    decimal sumCal = 0;
                    var list = new List<Dish>();
                    for (int i = 0; i < ListDish.Count(); i++)
                    {
                        list.Add(ListDish[i]);
                        if (i % 2 == 0)
                        {
                            var d = ListDish.First(p => p.Calories == ListDish.Max(p => p.Calories));
                            list.Add(d);
                            ListDish.Remove(d);

                        }
                        else
                            list.Add(ListDish[ListDish.Count() - 1 - i]);
                    }
                    foreach (var i in list)
                    {
                        if (meals.Where(p => p.Date == date).Count() <= card.MealCount)
                        {
                            var m = db.Menu.FirstOrDefault(p => p.IdDish == i.IdDish && p.IdCard == card.IdCard);
                            if (m == null)
                            {
                                continue;
                            }
                            meals.Add(new Meal { Date = date, IdMeal = idMeal, IdMenu = m.IdMenu });
                            idMeal++;
                            sumCal += i.Calories;
                            if (sumCal > card.DailyCalories)
                            {
                                break;
                            }
                        }
                        else
                        {
                            date += new TimeSpan(1, 0, 0, 0);
                        }

                        if (meals.Count() == 10 * card.MealCount)
                        {
                            break;
                        }
                    }

                    await db.Meals.AddRangeAsync(meals);
                    db.SaveChanges();
                }

                foreach (var i in meals)
                {
                    var d = db.Dishes.FirstOrDefault(p => p.IdDish == db.Menu.FirstOrDefault(p => p.IdMenu == i.IdMenu).IdDish);
                    List<Product> prod = new List<Product>();
                    foreach (var y in DishesProduct.SelectDishProd(d.IdDish))
                    {
                        prod.Add(db.Products.FirstOrDefault(p => p.IdProduct == y));
                    }
                    model.Add(new DiaryModel { meal = i, Dishes = d, productInDish = prod });
                }
            }

            return model;
        }
        public static async Task<List<DiaryModel>> selectMenu(int id)
        {
            DietDBContext db = new DietDBContext();
            List<DiaryModel> model = new List<DiaryModel>();
            PatientCard card = db.PatientCards.First(p => p.IdCard == db.Menu.First(t => t.IdMenu == id).IdCard);
            List<Menu> menu =await db.Menu.Where(p => p.IdCard == card.IdCard).ToListAsync();
            foreach(var m in menu)
            {
                var d = db.Dishes.First(p => p.IdDish == m.IdDish);
                var dp = db.DishesProducts.Where(p => p.IdDish == d.IdDish).ToList();
                var prod = new List<Product>();
                foreach(var i in dp)
                {
                    prod.Add(db.Products.First(p => p.IdProduct == i.IdProduct));
                }
                model.Add(new DiaryModel { Dishes = d, productInDish = prod });
            }
            return model;
        }

    }
}
