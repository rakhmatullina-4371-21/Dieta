﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

#nullable disable

namespace Diet.Models
{
    public partial class Dish
    {
        public int IdDish { get; set; }

        [RegularExpression(@"^[А-Яа-я ]+$"), Required(ErrorMessage = "Некорректные данные")]
        public string Dish1 { get; set; }

        [RegularExpression(@"[0-9]*\,?[0-9][0-9]", ErrorMessage = "Некорректные данные")]
        public decimal Calories { get; set; }

        [RegularExpression(@"[0-9]*\,?[0-9][0-9]", ErrorMessage = "Некорректные данные")]
        public decimal Protein { get; set; }

        [RegularExpression(@"[0-9]*\,?[0-9][0-9]", ErrorMessage = "Некорректные данные")]
        public decimal Fats { get; set; }

        [RegularExpression(@"[0-9]*\,?[0-9][0-9]", ErrorMessage = "Некорректные данные")]
        public decimal Carbohydrates { get; set; }

        static DietDBContext db = new DietDBContext();
        public static async Task<Dish> SelectDish(int id)
        {
            Dish dish = await db.Dishes.FirstOrDefaultAsync(p => p.IdDish == id);
            return dish;
        }
        public static async Task<int> MaxIdDish()
        {
            int ID = await db.Dishes.MaxAsync(p => p.IdDish) + 1;
            return ID;
        }
        public async Task SaveDish(Dish dish)
        {
            Dish di = await db.Dishes.FirstOrDefaultAsync(p => p.IdDish == dish.IdDish);
            Dish newDish = new Dish();
            newDish.IdDish = dish.IdDish;
            newDish.Dish1 = dish.Dish1;
            newDish.Calories = dish.Calories;
            newDish.Protein = dish.Protein;
            newDish.Fats = dish.Fats;
            newDish.Carbohydrates = dish.Carbohydrates;
            if (di == null)
            {
                await db.Dishes.AddRangeAsync(newDish);
            }
            db.SaveChanges();
        }

        public static List<Dish> SelectDishes()
        {
            return db.Dishes.Select(p => p).ToList();
        }
    }
}

