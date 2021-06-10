using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Diet.Models.ViewModels
{
    public class MenuPatientModel
    {
        public Dish dish { get; set; }
        public bool allowed { get; set; }


        public static async Task<List<MenuPatientModel>> MenuPatientSelect(int idPatient,int idEmp)
        {
            DietDBContext db = new DietDBContext();
            var card = await PatientCard.SelectPatientCard(idPatient, idEmp);
            var diagnPat = db.PatientDiagnoses.Where(p => p.IdCard == card.IdCard).ToList();
            List<int?> listProd = new List<int?>();
            List<MenuPatientModel> menuPatient = new List<MenuPatientModel>();
            foreach(var i in diagnPat)
            {
                listProd.AddRange(db.DiagnosesDishes.Where(p => p.IdDiagnosis == i.IdDiagnosis).Select(p => p.IdProduct));
            }
            listProd.Distinct();
            foreach(var i in db.Dishes)
            {
                if (listProd.Join(db.DishesProducts,p=>p,t=>t.IdProduct,(p,t)=>p).Contains(i.IdDish))
                {
                    
                }
            }
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
