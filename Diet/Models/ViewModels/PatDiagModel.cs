using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Diet.Models.ViewModels
{
    public class PatDiagModel
    {
        public int idCard { get;set; }
        public List<bool> diagnPat { get; set; }
        public List<int> idDiag { get; set; }

    }
}
