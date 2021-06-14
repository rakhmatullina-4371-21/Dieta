﻿using Diet.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

#nullable disable

namespace Diet.Models
{
    public partial class PatientIndicator
    {
        public int id { get; set; }
        public int? IdCard { get; set; }
        public int? IdIndicator { get; set; }

        public string ValueIndicator { get; set; }
        public bool? Result { get; set; }
        [Display(Name = "DateIndicator")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime DateIndicator { get; set; }

        

        public virtual PatientCard IdCardNavigation { get; set; }
        public virtual Indicator IdIndicatorNavigation { get; set; }

        static DietDBContext db = new DietDBContext();
        public static async Task<List<PatientIndicator>> listPatIndicators( int id)
        {
            db = new DietDBContext();
            var card = await db.PatientCards.FirstOrDefaultAsync(p=>p.IdPatient==id);
            var list = db.PatientIndicators.Where(p => p.IdCard == card.IdCard).Select(p=>p.DateIndicator).Distinct().ToList();
            var dist = new List<PatientIndicator>();
           foreach (var t in list)
            {
                dist.Add(db.PatientIndicators.First(p => p.DateIndicator == t));
            }
            return dist;
        }
        public static async Task<int?> SaveAnalysisAsync(List<AnalysisModel> patIndicator, int idCard)
        {
            List<PatientIndicator> _patientIndicators = new List<PatientIndicator>();
            var update = new List<PatientIndicator>();
            var add = new List<PatientIndicator>();
            foreach (var item in patIndicator)
            {
                bool result = true;
                var ind = await db.Indicators.FirstOrDefaultAsync(p => p.IdIndicator == item.IdIndicator);
                if ((ind.Max != null && ind.Min != null) || (ind.Min != null || ind.Min != null))
                {
                    if (item.Value > decimal.Parse(ind.Max) || item.Value < decimal.Parse(ind.Min))
                    {
                        result = false;
                    }
                }

                PatientIndicator patientIndicator = new PatientIndicator() { IdCard = idCard, IdIndicator = item.IdIndicator, /*IdIndicatorNavigation = db.Indicators.FirstOrDefault(p => p.IdIndicator == item.IdIndicator),*/ ValueIndicator = item.Value.ToString(), DateIndicator = DateTime.Now, Result = result };
 
                var tmp = db.PatientIndicators.Where(p => p.IdCard == patientIndicator.IdCard && p.DateIndicator== item.dateIndicator).ToList();
                //foreach(var i in tmp)
                //{
                //    if (i.DateIndicator != patientIndicator.DateIndicator)
                //    {
                //        if(tmp.Remove(i)!=null)
                //        {
                //            tmp.Remove(i);
                //        } else 
                //        catch { tmp = new List<PatientIndicator>(); }
                //    }
                //}
                if (tmp.Count == 0)
                {
                    add.Add(patientIndicator);
                }
                else
                {
                    
                    //patientIndicator.id = ;
                    update.Add(patientIndicator);
                }
            }
            db.SaveChanges();
            foreach (var t in add)
            {
                DietDBContext DB = new DietDBContext();
                if (db.PatientIndicators.Count() != 0)
                {
                    t.id = DB.PatientIndicators.Max(p => p.id) + 1;
                }
                else t.id = 1;
                DB.PatientIndicators.Add(t);
                DB.SaveChanges();
            }
            foreach (var t in update)
            {

                DietDBContext DB = new DietDBContext();
                t.id = db.PatientIndicators.FirstOrDefault(p => p.IdCard == t.IdCard && p.IdIndicator == t.IdIndicator && p.DateIndicator == t.DateIndicator).id;
                DB.PatientIndicators.Update(t);
                DB.SaveChanges();
            }



            var d = db.PatientCards.First(p => p.IdCard == idCard).IdPatient;
            return d;
        }
    }
}
