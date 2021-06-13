using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Diet.Models.ViewModels
{
    public class ListProductsInDishModel
    {
        [Required(ErrorMessage = "")]
        public Dish OneDish { get; set; }

        public List<bool> ProductInDish { get; set; }
        public List<Product> listProduct { get; set;}








        static DietDBContext db = new DietDBContext();

        public static List<Product> ListProduct()
        {
            return db.Products.Select(p=>p).ToList(); 
        }
        public static List<bool> ListProductItem(int id)
        {
            List< bool> item = new List<bool>();
           var ListIdProd= db.DishesProducts.Where(p => p.IdDish == id).Select(p => p.IdProduct).ToList();
            foreach (var t in db.Products)
            {
                var prod = int.Parse(t.IdProduct.ToString());
                 item.Add(ListIdProd.Contains(prod));
            }
            return item;
        }


        public static async Task<ListProductsInDishModel> ListInProd(int idDish)
        {
            ListProductsInDishModel ListProdDish = new ListProductsInDishModel();
            ListProdDish.OneDish = await Dish.SelectDish(idDish);
            if (ListProdDish.OneDish == null) { ListProdDish.OneDish = new Dish(); ListProdDish.OneDish.IdDish = await Dish.MaxIdDish(); }
            ListProdDish.listProduct = ListProduct().OrderBy(p=>p.Product1).ToList();
            ListProdDish.ProductInDish = ListProductItem(idDish);
            return ListProdDish;
        }

        public static  void SaveDish(ListProductsInDishModel model)
        {
            db = new DietDBContext();
            Dish dish = new Dish();
            dish.IdDish= model.OneDish.IdDish;
            dish.Dish1 = model.OneDish.Dish1;
            dish.Calories = model.OneDish.Calories;
            dish.Fats = model.OneDish.Fats;
            dish.Protein = model.OneDish.Protein;
            dish.Carbohydrates = model.OneDish.Carbohydrates;

            if(db.Dishes.Select(p=>p.IdDish).Contains(model.OneDish.IdDish))
            {
                db.Dishes.Update(dish);
            }
            else
            {
                 db.Dishes.Add(dish);
            }
              db.SaveChanges();

        }
        public static  void SaveProductDish(ListProductsInDishModel model, Dish dish)
        {
            DietDBContext dBContext = new DietDBContext();
            List<Product> ProductList = dBContext.Products.Select(p => p).ToList();
            int MaxId = dBContext.DishesProducts.Max(p => p.id)+1;
            List<DishesProduct> save = new List<DishesProduct>();
            List<DishesProduct> del = new List<DishesProduct>();

            for (int i = 0; i < ProductList.Count(); i++)
            {
                DishesProduct dp = new DishesProduct();
                var idProd = ProductList[i];
                dp.id = MaxId + 1;
                dp.IdDish = dish.IdDish;
                dp.IdProduct = idProd.IdProduct;
                
                if (model.ProductInDish[i] == true)
                {
                    if (dBContext.DishesProducts.FirstOrDefault(p => p.IdDish == dp.IdDish && p.IdProduct == dp.IdProduct) == null)
                    {
                        //dBContext.DishesProducts.Add(dp);
                        save.Add(dp);
                        MaxId++;
                    }
                }
                else
                {
                    if (dBContext.DishesProducts.FirstOrDefault(p => p.IdDish == dp.IdDish && p.IdProduct == dp.IdProduct) != null)
                    {
                        dp.id = dBContext.DishesProducts.FirstOrDefault(p => p.IdDish == dp.IdDish && p.IdProduct == dp.IdProduct).id;
                        del.Add(dp);

                    }
                }
                
            }
            dBContext.DishesProducts.AddRange(save);
            dBContext.SaveChanges();
            DelProductDish(del);
        }
        public static  void DelProductDish(List<DishesProduct> del)
        {
            DietDBContext dBContext = new DietDBContext();
            dBContext.RemoveRange(del);
            dBContext.SaveChanges();

        }


    }
}
