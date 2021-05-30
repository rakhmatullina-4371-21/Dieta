using System;
using System.Collections.Generic;

#nullable disable

namespace Diet.Models
{
    public partial class Indicator
    {
        public int IdIndicator { get; set; }
        public string NameIndicator { get; set; }
        public bool? Laboratory { get; set; }
        public string Min { get; set; }
        public string Max { get; set; }


        
    }
}
