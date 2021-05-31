using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Diet.Models.ViewModels
{
    public class AnalysisModel
    {
        public int IdIndicator { get; set;}
        public string NameIndicator { get; set; }

        [RegularExpression(@"[0-9]*\,?[0-9][0-9]", ErrorMessage = "Некорректные данные")]
        public decimal Value { get; set; }
        

        public static List<AnalysisModel> SelectIndicatorsValue(int idCard, string date)
        {
            DietDBContext db = new DietDBContext();
            List<AnalysisModel> analyses = new List<AnalysisModel>();
            if (date==null)
            {
                foreach (var i in db.Indicators.Select(p => p))
                {
                    analyses.Add(new AnalysisModel { IdIndicator = i.IdIndicator, NameIndicator = i.NameIndicator });
                }
            }
            else 
            {
                var an = db.PatientIndicators.Where(p => p.IdCard == idCard && p.DateIndicator == DateTime.Parse(date)).ToList();
                an.Reverse();
                an=an.Take(db.Indicators.Count()).ToList();
                an.Reverse();
                var indicator = db.Indicators.Select(p=>p).ToList();
                for (int i = 0; i < indicator.Count() ; i++)
                {
                    analyses.Add(new AnalysisModel { IdIndicator = indicator[i].IdIndicator, Value= decimal.Parse(db.PatientIndicators.First(p=>p.id==an[i].id).ValueIndicator), NameIndicator = indicator[i].NameIndicator });
                }
            }

            return analyses;
        }
    }
}
