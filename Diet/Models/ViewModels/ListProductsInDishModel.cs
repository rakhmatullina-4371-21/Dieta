using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Diet.Models.ViewModels
{
    public class ListProductsInDishModel
    {
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
            if (ListProdDish.OneDish == null) { ListProdDish.OneDish.IdDish = await Dish.MaxIdDish(); }
            ListProdDish.listProduct = ListProduct();
            ListProdDish.ProductInDish = ListProductItem(idDish);
            return ListProdDish;
        }
    }
}
