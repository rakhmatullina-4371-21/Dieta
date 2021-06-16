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
            Meals = new HashSet<Meal>();
        }

        public int IdMenu { get; set; }
        public int? IdCard { get; set; }
        public int? IdDish { get; set; }

        public virtual PatientCard IdCardNavigation { get; set; }
        public virtual Dish IdDishNavigation { get; set; }
        public virtual ICollection<Meal> Meals { get; set; }
        public static List<Menu> list()
        {
            DietDBContext db = new DietDBContext();
            return db.Menu.Select(p => p).ToList();

            }



    }
}
