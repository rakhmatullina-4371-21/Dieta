using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable

namespace Diet.Models
{
    public partial class Menu
    {
        public Menu()
        {
            Meals = new HashSet<Meals>();
        }
        public int IdPosition { get; set; }
        public int? IdCard { get; set; }
        public string Dish { get; set; }
        public decimal? Calories { get; set; }

        public decimal? Protein { get; set; }
        public decimal? Fats { get; set; }
        public decimal? Carbohydrates { get; set; }
        public virtual PatientCard IdCardNavigation { get; set; }
        public virtual ICollection<Meals> Meals { get; set; }



        public static List<Menu> list()
        {
            DietDBContext db = new DietDBContext();
            return db.Menu.Select(p => p).ToList();

            }



    }
}
