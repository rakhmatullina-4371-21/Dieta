using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Diet.Models.ViewModels
{
    public class MenuDishPatientModel
    {
        public Dish dish { get; set; }
        public bool allow { get; set; }

        public int idCard { get; set; }

        public static async Task<List<MenuDishPatientModel>> DishPatientSelect(List<MenuPatientModel> model)
        {
            DietDBContext db = new DietDBContext();
            List<int> listProd = new List<int>();
            List<MenuDishPatientModel> menuPatient = new List<MenuDishPatientModel>();
            foreach (var i in model)
            {
                if (i.allowed == false)
                {
                    listProd.Add(i.prod.IdProduct);
                }
            }

            foreach(var i in db.Dishes.OrderBy(p=>p.Dish1))
            {
                menuPatient.Add(new MenuDishPatientModel { dish = i });
            }
            foreach(var i in menuPatient)
            {
                var prodInDish = db.DishesProducts.Where(p => p.IdDish == i.dish.IdDish).Select(p => p);
                foreach (var j in prodInDish)
                {
                    if (listProd.Contains(Convert.ToInt32(j.IdProduct)))
                    {
                        i.allow = false;
                        break;
                    }
                    else
                    {
                        i.allow = true;

                    }
                }
            }
            
            return menuPatient;
        }
        public static  void DelMenu(List<MenuDishPatientModel> model)
        {
            DietDBContext db = new DietDBContext();
            if(db.Menu.Where(p => p.IdCard == model.ElementAt(0).idCard).Count() != 0)
            {
                var t = db.Menu.Where(p => p.IdCard == model.ElementAt(0).idCard).ToList();
                List<Meal> listM = new List<Meal>();
                foreach (var u in t)
                {
                    listM.AddRange(db.Meals.Where(p => p.IdMenu == u.IdMenu));
                }
                db.Meals.RemoveRange(listM);
                db.SaveChanges();
                db.Menu.RemoveRange(t);
                db.SaveChanges();
            }
           
        }
            public static async Task SaveMenu(List<MenuDishPatientModel> model)
        {
            DietDBContext db = new DietDBContext();
           
            List<Menu> menu = new List<Menu>();
            var id = 1;
            if (db.Menu.Count() != 0)
            {
                 id = db.Menu.Max(p => p.IdMenu) + 1;
            }
            foreach (var i in model)
            {
                if (i.allow== true)
                {
                    i.dish = db.Dishes.FirstOrDefault(p => p.IdDish == i.dish.IdDish);
                    if (db.Menu.Count() == 0)
                    {
                        menu.Add(new Menu { IdMenu = id, IdCard = i.idCard, IdDish = Convert.ToInt32(i.dish.IdDish)});
                    }
                    else
                    {
                        menu.Add(new Menu { IdMenu = id, IdCard = i.idCard, IdDish = Convert.ToInt32(i.dish.IdDish)});
                    }
                    id++;
                }
            }
            foreach (var i in menu)
            {
                db.Menu.Add(i);
                db.SaveChanges();
            }

        }
    }
}
