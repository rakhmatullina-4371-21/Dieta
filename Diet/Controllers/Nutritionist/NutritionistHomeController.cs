using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Diet.Models;
using Diet.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Diet.Controllers.Nutritionist
{
    [Authorize]
    public class NutritionistHomeController : Controller
    {

        public async Task<IActionResult> CardOnePat( int id)                              //Просмотр карты пациента
        {
            var card =await PatientCard.SelectPatientCard(id, int.Parse(HttpContext.User.Identity.Name));
            return View(card);
        }
        public IActionResult MenuNutritionist()                                 //Стартовое окно диетолога
        { 
            var id = HttpContext.User.Identity.Name;
            var list = PatientCard.SelectPatientsNutr(int.Parse(id));
            return View(list);
        }
        public IActionResult PatSelect()                                           //Список пациентов
        {
            return View(Patient.SelectPatientCardNull(int.Parse(HttpContext.User.Identity.Name)));
        }

        public IActionResult MenuPatient(int id)
        {
            return View(Diagnosis.ListDiagnosis(id));
        }

        public IActionResult DiagnosisPatient(int id)             //Диагнозы пациента
        {
            return View(Diagnosis.ListDiagnosis(id));
        }

        public IActionResult DiagnosisSave(PatDiagModel diagnosis)      //Сохранение диагнозов пациента
        {
            if (ModelState.IsValid)
            {
                  Diagnosis.SavePatDiag(diagnosis);
                return Redirect("~/NutritionistHome/MenuNutritionist");
            }
            return View();
        }


        [HttpGet]
        public IActionResult CardPatient(int id)        //Создание карты пациента
        {
            PatientCard card = new PatientCard();
            card.IdPatient = id;
            return  View(card);
        }
        [HttpPost]
        public async Task<IActionResult> CardPatient(PatientCard card)          //Сохрание карты пациента
        {
            if (ModelState.IsValid )
            {
                
               var idCard= await PatientCard.SavePatCard(int.Parse(HttpContext.User.Identity.Name), card);
                return Redirect("~/NutritionistHome/MenuNutritionist");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> OnePatient(int id)                            //Добавление нового пациента
        {
            Patient patient = await Patient.SelectPatient(id);
            if (patient == null) { patient = new Patient(); patient.IdPatient = await Patient.MaxIdPatient(); }
            return View(patient);
        }
        [HttpPost]
        public async Task<IActionResult> OnePatient(Patient patient)                            //Сохраниение данных нового пациента
        {
            if (ModelState.IsValid)
            {
                if (await Patient.LoginContains(patient.Login))
                {
                    await patient.SavePatient(patient);
                   return Redirect("~/NutritionistHome/PatSelect");
                }
                else 
                    ModelState.AddModelError("", "Такой логин уже существует");
            }
            return View(patient);
        }

        public async Task<IActionResult> IndicatorPatient(int id)             //Страница показателей одного пациента
        {
            var list = await PatientIndicator.listPatIndicators(id);
            ViewBag.idCard = id;
            return View(list);
        }




        [HttpGet]
        public IActionResult OneIndicatorPatient(int id, string date)     // Добавить значения показателей (анализ)
        {
            var list = AnalysisModel.SelectIndicatorsValue(id, date);
            ViewBag.idCard = id;
            return View(list);
        }
        [HttpPost]
        public async Task<IActionResult> OneIndicatorPatient(List<AnalysisModel> patIndicator, int id)    //Сохрранение значений показателей
        {
            if (ModelState.IsValid)
            {
                await PatientIndicator.SaveAnalysisAsync(patIndicator, id);
                return Redirect("~/NutritionistHome/MenuNutritionist");
            }
            ViewBag.idCard = id;
            return View(patIndicator);
        }


        public async Task<IActionResult> MenuPat(int id)                            //Просмотр разрешенных продуктов пациента
        {
            return View( await MenuPatientModel.ProductPatientSelect(id,int.Parse(HttpContext.User.Identity.Name)));
        }


   
        public async Task<IActionResult> MenuPatDishes(List<MenuPatientModel> model)                            //Просмотр разрешенных блюд пациента
        {
            return View(await MenuDishPatientModel.DishPatientSelect(model));
        }
   
        public async Task<IActionResult> SaveMenuPat(List<MenuDishPatientModel> model)                            //Просмотр разрешенных блюд пациента
        {
            await MenuDishPatientModel.SaveMenu(model);
            return Redirect("~/NutritionistHome/MenuNutritionist");
        }
    }

    
}
