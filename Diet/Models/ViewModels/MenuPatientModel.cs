using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Diet.Models.ViewModels
{
    public class MenuPatientModel
    {
        public Product prod { get; set; }
        public bool allowed { get; set; }

        public int idCard { get; set; }
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
                    menuPatient.Add(new MenuPatientModel {prod=t, idCard=card.IdCard, allowed=false});
                }
                else
                {
                    menuPatient.Add(new MenuPatientModel { prod = t, idCard=card.IdCard, allowed = true });
                }
            }
            return menuPatient.OrderBy(p=>p.prod.Product1).ToList();
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
