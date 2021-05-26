using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable disable

namespace Diet.Models
{
    public partial class Position
    {
        public Position()
        {
            Employees = new HashSet<Employee>();
        }

        public int IdPosition { get; set; }
        public string Position1 { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }

        static DietDBContext db = new DietDBContext();


        public static List<Position> SelectPosition() 
        {
            return db.Positions.Select(p => p).ToList();
        }
    }
}
