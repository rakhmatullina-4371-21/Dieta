﻿using Diet.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

#nullable disable

namespace Diet.Models
{
    public partial class PatientCard
    {
        public PatientCard()
        {
            Meals = new HashSet<Meal>();
        }

        public int IdCard { get; set; }
        public int? IdPatient { get; set; }
        public int? IdEmployee { get; set; }
        [RegularExpression(@"[0-9]*\,?[0-9][0-9]", ErrorMessage = "Некорректные данные")]
        public decimal? DailyCalories { get; set; }
        [RegularExpression(@"[0-9]*\,?[0-9][0-9]", ErrorMessage = "Некорректные данные")]
        public decimal? DailyProtein { get; set; }
        [RegularExpression(@"[0-9]*\,?[0-9][0-9]", ErrorMessage = "Некорректные данные")]
        public decimal? DailyFats { get; set; }
        [RegularExpression(@"[0-9]*\,?[0-9][0-9]", ErrorMessage = "Некорректные данные")]
        public decimal? DailyCarbohydrates { get; set; }
        public bool Activ { get; set; }

        [Display(Name = "Start")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? StartDiet { get; set; }

        [Display(Name = "Fihish")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]

        public DateTime? FinishDiet { get; set; }
        public int? IdActivityLevels { get; set; }

        public virtual ActivityLevel IdActivityLevelsNavigation { get; set; }
        public virtual Employee IdEmployeeNavigation { get; set; }
        public virtual Patient IdPatientNavigation { get; set; }
        public virtual ICollection<Meal> Meals { get; set; }


        static DietDBContext db = new DietDBContext();
        public static async Task<int> SavePatCard(int idEmployee, PatientCard patientCard)
        {
            PatientCard card = new PatientCard();
            var listCard = db.PatientCards.Where(p => p.IdPatient == patientCard.IdPatient && p.IdEmployee == idEmployee).Select(p => p);
            var countCards = await db.PatientCards.CountAsync();
            card.IdPatient = patientCard.IdPatient;
            card.IdEmployee = idEmployee;

            card.IdActivityLevels = patientCard.IdActivityLevels;
            if (countCards == 0)
            {
                card.IdCard = 1;
            }
            else if (patientCard.IdCard == 0)
            {
                card.IdCard = await db.PatientCards.MaxAsync(p => p.IdCard) + 1;
            }
            else {
                card.IdCard = patientCard.IdCard;
            }
            var t =  db.PatientCards.FirstOrDefault(p => p.IdEmployee == card.IdEmployee && p.IdPatient == card.IdPatient);

            if (t==null)
            {
                card.Activ = true;
                db = new DietDBContext();
                 db.PatientCards.Add(card);
                
            } else
            {
                t.IdEmployee = card.IdEmployee;
                t.IdPatient = card.IdPatient;
                t.Meals = card.Meals;
                t.StartDiet = card.StartDiet;
                t.FinishDiet = card.FinishDiet;
                t.Activ = true;
                t.DailyCalories = card.DailyCalories;
                t.DailyCarbohydrates = card.DailyCarbohydrates;
                t.DailyFats = card.DailyFats;
                t.DailyProtein = card.DailyProtein;
                t.IdActivityLevels = card.IdActivityLevels;
                await SaveMenu(t.IdCard);

                db.PatientCards.Update(t);
                card.IdCard = t.IdCard;
            }

            db.SaveChanges();
            return card.IdCard;
        }

        public static List<Patient> SelectPatientsNutr( int idEmp)
        {
            var pat = (from t in db.Patients
                       join r in db.PatientCards on t.IdPatient equals r.IdPatient
                       where r.IdEmployee == idEmp && r.Activ==true
                       select t).Distinct().ToList();
            return pat;
        }
        public static async Task<PatientCard> SelectPatientCard(int idPat, int idEmp)
        {
            var c = db.PatientCards.FirstOrDefault(p => p.IdPatient == idPat && p.IdEmployee == idEmp && p.Activ == true);
            if (c.StartDiet == null)
            {
                c.StartDiet = DateTime.Now;
            }
                c =await DailyCCPF(c);
            return c;
        }

        public static async Task<PatientCard> DailyCCPF( PatientCard card)
        {
            var patient = await db.Patients.FirstOrDefaultAsync(p=>p.IdPatient==card.IdPatient);
            var activity = db.ActivityLevels.Where(p => p.IdActivityLevels == card.IdActivityLevels).Select(p => p.Value).First();
            var dateAnalyses = db.PatientIndicators.Max(p => p.DateIndicator);
            decimal height =Convert.ToDecimal(db.Indicators.Join(db.PatientIndicators, p => p.IdIndicator, t => t.IdIndicator, (p, t) => new { t.ValueIndicator, p.NameIndicator, t.IdCard, t.DateIndicator }).FirstOrDefault(p => p.NameIndicator == "Рост" && p.IdCard==card.IdCard && p.DateIndicator==dateAnalyses).ValueIndicator);
            decimal weight =Convert.ToDecimal(db.Indicators.Join(db.PatientIndicators, p => p.IdIndicator, t => t.IdIndicator, (p, t) => new { t.ValueIndicator, p.NameIndicator , t.IdCard, t.DateIndicator }).FirstOrDefault(p => p.NameIndicator == "Вес" && p.IdCard == card.IdCard && p.DateIndicator == dateAnalyses).ValueIndicator);
            var age = DateTime.Now.Year - patient.DateOfBirth.Value.Year;
            if (DateTime.Now.DayOfYear < patient.DateOfBirth.Value.DayOfYear)
            {
                age--;
            }
            if (patient.Woman == true)
            {
                if(height==0 || weight == 0)
                {
                    if (age <= 25)
                    {
                        card.DailyCalories = 2100;
                    }
                    else if (age >= 26 && age < 50)
                    {
                        card.DailyCalories = 1900;
                    }
                    else if (age >= 50)
                    {
                        card.DailyCalories = 1700;
                    }
                }
                else
                {
                    card.DailyCalories = (10 * weight) + (Convert.ToDecimal(6.25) * height) - (5 * age) - 161;
                }
               
            }
            else if (patient.Woman == false)
            {
                if (height == 0 || weight == 0)
                {
                    if (age < 30)
                    {
                        card.DailyCalories = 2500;
                    }
                    else if (age >= 31 && age < 50)
                    {
                        card.DailyCalories = 2300;
                    }
                    else if (age >= 50)
                    {
                        card.DailyCalories = 2000;
                    }
                }
                else
                {
                    card.DailyCalories = (10 * weight) + (Convert.ToDecimal(6.25) * height) - (5 * age) + 5;
                }
                  
            }
            card.DailyCalories *=  activity;
            card.DailyCarbohydrates= card.DailyCalories * Convert.ToDecimal(0.6);
            card.DailyProtein = card.DailyCalories * Convert.ToDecimal(0.15);
            card.DailyFats = card.DailyCalories * Convert.ToDecimal(0.25);

            return card;
        }
    }
}
