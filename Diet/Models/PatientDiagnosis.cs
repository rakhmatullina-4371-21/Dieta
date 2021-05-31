using System;
using System.Collections.Generic;

#nullable disable

namespace Diet.Models
{
    public partial class PatientDiagnosis
    {
        public int id { get; set; }
        public int? IdCard { get; set; }
        public int? IdDiagnosis { get; set; }

        public virtual PatientCard IdCardNavigation { get; set; }
        public virtual Diagnosis IdDiagnosisNavigation { get; set; }



    }
}
