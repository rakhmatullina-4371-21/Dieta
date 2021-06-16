using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable

namespace Diet.Models
{
    public partial class Indicator
    {
        public int IdIndicator { get; set; }
        public string NameIndicator { get; set; }
        public string Min { get; set; }
        public string Max { get; set; }
     
        public static List<Indicator> SelectIndicator()
        {
            DietDBContext db = new DietDBContext();
            var list = db.Indicators.Where(p=>p.NameIndicator=="Рост" || p.NameIndicator=="Вес" ).Select(p => p).ToList();
            return list;
        }
        
        
    }
}
