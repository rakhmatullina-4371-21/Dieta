using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Diet.Models.ViewModels
{
    public class AddProductModel
    {
        public int IdProduct { get; set; }
        [RegularExpression(@"^[А-Яа-я ]+$"), Required(ErrorMessage = "Некорректные данные"), StringLength(15)]
        public string Product1 { get; set; }
        public List<Diagnosis> diagnoses { get; set; }
        public List<bool> diagProd { get; set; }

        static DietDBContext db = new DietDBContext();

        public static async Task<AddProductModel> ProductInDish(int idProduct)
        {

            AddProductModel t = new AddProductModel();
            t.diagnoses = await db.Diagnoses.Select(p => p).OrderBy(p=>p.NameDiagnosis).ToListAsync();
            t.IdProduct = idProduct;
            t.diagProd = new List<bool>();
            if(db.Products.FirstOrDefault(p => p.IdProduct == idProduct)!= null)
            {
                t.Product1 = db.Products.FirstOrDefault(p => p.IdProduct == idProduct).Product1;
            } 

            if (db.DiagnosesDishes.Where(p => p.IdProduct == idProduct).Count() != 0)
            {
                db.DiagnosesDishes.RemoveRange(db.DiagnosesDishes.Where(p => p.IdProduct ==t.IdProduct));
                var list = db.DiagnosesDishes.Where(p => p.IdProduct == idProduct).Select(p=>p.IdDiagnosis).ToList();
                foreach (var i in t.diagnoses)
                {
                    if (list.Contains(i.IdDiagnosis))
                    {
                        t.diagProd.Add(true);
                    }
                    else
                    {
                        t.diagProd.Add(false);
                    }
                }
            }
            else
            {
                for(int i=0;i<t.diagnoses.Count();i++)
                {
                    t.diagProd.Add(false);
                }
            }

            return t;
        }
    }
}
