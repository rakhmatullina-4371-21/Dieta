using System;
using System.Collections.Generic;

#nullable disable

namespace Diet.Models
{
    public partial class PatientIndicator
    {
        public int? IdCard { get; set; }
        public int? IdIndicator { get; set; }
        public string ValueIndicator { get; set; }
        public bool? Result { get; set; }
        public DateTime DateIndicator { get; set; }

        public virtual PatientCard IdCardNavigation { get; set; }
        public virtual Indicator IdIndicatorNavigation { get; set; }
    }
}
