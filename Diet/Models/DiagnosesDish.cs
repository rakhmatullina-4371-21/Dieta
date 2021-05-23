using System;
using System.Collections.Generic;

#nullable disable

namespace Diet.Models
{
    public partial class DiagnosesDish
    {
        public int? IdDiagnosis { get; set; }
        public int? IdProduct { get; set; }
        public bool? Allowed { get; set; }

        public virtual Diagnosis IdDiagnosisNavigation { get; set; }
        public virtual Product IdProductNavigation { get; set; }
    }
}
