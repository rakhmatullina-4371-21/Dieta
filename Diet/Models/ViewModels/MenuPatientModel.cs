using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Diet.Models.ViewModels
{
    public class MenuPatientModel
    {
        public Product prod { get; set; }
        public Dish dish { get; set; }
        public bool allowed { get; set; }


        public static async Task<List<MenuPatientModel>> ProductPatientSelect(int idPatient,int idEmp)
        {
            DietDBContext db = new DietDBContext();
            var card = await PatientCard.SelectPatientCard(idPatient, idEmp);
            var diagnPat = db.PatientDiagnoses.Where(p => p.IdCard == card.IdCard).ToList();
            List<DiagnosesDish> listProd = new List<DiagnosesDish>();
            List<MenuPatientModel> menuPatient = new List<MenuPatientModel>();
            foreach(var i in diagnPat)
            {
                listProd.AddRange(db.DiagnosesDishes.Where(p => p.IdDiagnosis == i.IdDiagnosis && p.Allowed==false).Select(p => p));
            }
            listProd.Distinct();
            foreach(var t in db.Products)
            {
                if (listProd.FirstOrDefault(p => p.IdProduct == t.IdProduct) != null)
                {
                    menuPatient.Add(new MenuPatientModel {prod=t, allowed=false});
                }
                else
                {
                    menuPatient.Add(new MenuPatientModel { prod = t, allowed = true });
                }
            }
            return menuPatient;
        }
        
        public static async Task<List<MenuPatientModel>> DishPatientSelect(List<MenuPatientModel> model)
        {
            DietDBContext db = new DietDBContext();
            List<int?> listProd = new List<int?>();
            List<MenuPatientModel> menuPatient = new List<MenuPatientModel>();
            foreach (var i in model)
            {
                if (i.allowed == false)
                {
                    listProd.Add(i.prod.IdProduct);
                }
            }

            foreach(var i in db.Dishes)
            {
                menuPatient.Add(new MenuPatientModel { dish = i });
            }
            foreach(var i in menuPatient)
            {
                var prodInDish = db.DishesProducts.Where(p => p.IdDish == i.dish.IdDish).Select(p => p).ToList();
                foreach (var j in prodInDish)
                {
                    if (listProd.Contains(j.IdProduct))
                    {
                        i.allowed = false;
                        break;
                    }
                    else
                    {
                        i.allowed = true;

                    }
                }
            }
            return menuPatient;
        }
        public static async Task SaveMenu(int idCard)
        {
            DietDBContext db = new DietDBContext();
            var listDiag = db.PatientDiagnoses.Where(p => p.IdCard == idCard).Select(p => p.IdDiagnosis).ToList();
            var listProd = new List<DiagnosesDish>();
            var listProdAllow = new List<DiagnosesDish>();

            var listDish = new List<Dish>();

            foreach (var i in listDiag)
            {
                listProd.AddRange(db.DiagnosesDishes.Where(p => p.IdDiagnosis == i && p.Allowed == false).Select(p => p).ToList());
                listProdAllow.AddRange(db.DiagnosesDishes.Where(p => p.IdDiagnosis == i && p.Allowed == true).Select(p => p).ToList());

            }

            var list = listProdAllow.Except(listProd).Select(p => Convert.ToInt32(p.IdProduct)).Distinct().ToList();
            if (listProdAllow.Count == 0)
            {
                list.AddRange(db.Products.Select(p => p.IdProduct).ToList());
            }
            foreach (var i in list)
            {
                var prod = db.DishesProducts.Where(p => p.IdProduct == i).Select(p => p.IdDish).Distinct().ToList();
                foreach (var j in prod)
                {
                    listDish.Add(db.Dishes.FirstOrDefault(p => p.IdDish == j));
                }
            }

        }
    }
}
