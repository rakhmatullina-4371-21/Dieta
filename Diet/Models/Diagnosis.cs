using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Linq;
using Diet.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Diet.Models.ViewModels;

#nullable disable

namespace Diet
{
    public partial class Diagnosis
    {
        public int IdDiagnosis { get; set; }
        public string NameDiagnosis { get; set; }

        static  DietDBContext db = new DietDBContext();

        public static PatDiagModel ListDiagnosis( int idCard)
        {
            List<bool> diagnosisPat = new List<bool>();
            var patientDiagnosis = db.PatientDiagnoses.Where(p => p.IdCard == idCard).Select(p => p).Select(p=>p).ToList();
            foreach (var item in db.Diagnoses.Select(p=>p))
            {
                if (patientDiagnosis.Count() != 0)
                {
                    if(patientDiagnosis.Where(p => p.IdDiagnosis == item.IdDiagnosis).Count()!=0)
                    {
                        diagnosisPat.Add(true);
                    }
                    else
                    {
                        diagnosisPat.Add(false);
                    }
                }
                else
                {
                    diagnosisPat.Add( false);
                }
            }
            PatDiagModel model = new PatDiagModel();
            model.diagnPat = diagnosisPat;
            model.idDiag = db.Diagnoses.Select(p => p.IdDiagnosis).ToList();
            model.idCard = idCard;
            return model;
           
        }

        public static List<Diagnosis> ReturnDiagnosis()
        {
            return db.Diagnoses.Select(p => p).ToList();

        }

        public static void SavePatDiag(PatDiagModel model)
        {
            List<PatientDiagnosis> patdiag = new List<PatientDiagnosis>();
            for (int i = 0; i < model.idDiag.Count; i++)
            {
                int id = 1;
                if(db.PatientDiagnoses.Count()!=0)
                {
                     id = db.PatientDiagnoses.Max(p => p.id) + 1;
                } 
                if (model.diagnPat[i] == true)
                {
                    var idCardPat = db.PatientDiagnoses.FirstOrDefault(p => p.IdDiagnosis == model.idDiag[i] && p.IdCard ==model.idCard);
                    if (idCardPat != null)
                    {
                        db = new DietDBContext();
                        db.PatientDiagnoses.Update(new PatientDiagnosis { IdCard = model.idCard, IdDiagnosis = model.idDiag[i], id = idCardPat.id });
                        db.SaveChanges();
                    }
                    else
                    {
                        db = new DietDBContext();
                        db.PatientDiagnoses.Add(new PatientDiagnosis { IdCard =model.idCard, IdDiagnosis = model.idDiag[i], id = id });
                        db.SaveChanges();
                    }
                }
                PatientCard.SaveMenu(model.idCard);
                
            }
           
        }
    }
}
