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

        public IActionResult CardOnePat( int id)
        {
            var card = PatientCard.SelectPatientCard(id, int.Parse(HttpContext.User.Identity.Name));
            ViewData["diagnosis"] = Diagnosis.ListDiagnosis(card.IdCard);
            return View(card);
        }
        public IActionResult MenuNutritionist()
        { 
            var id = HttpContext.User.Identity.Name;
            var list = PatientCard.SelectPatientsNutr(int.Parse(id));
            return View(list);
        }
        public IActionResult PatSelect()
        {
            return View(Patient.SelectPatientCardNull(int.Parse(HttpContext.User.Identity.Name)));
        }
        [HttpGet]
        public IActionResult DiagnosisPatient(int id)
        {
            return View(Diagnosis.ListDiagnosis(id));
        }
        [HttpPost]
        public async Task<IActionResult> DiagnosisSave(PatDiagModel diagnosis)
        {
            if (ModelState.IsValid)
            {
                 Diagnosis.SavePatDiag(diagnosis);
                return Redirect("~/NutritionistHome/MenuNutritionist");
            }
            return View();
        }


        [HttpGet]
        public IActionResult CardPatient(int id)
        {
            PatientCard card = new PatientCard();
            card.IdPatient = id;
            return  View(card);
        }
        [HttpPost]
        public async Task<IActionResult> CardPatient(PatientCard card)
        {
            if (ModelState.IsValid)
            {
                await PatientCard.SavePatCard(int.Parse(HttpContext.User.Identity.Name), card);
                return Redirect("~/NutritionistHome/MenuNutritionist");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> OnePatient(int id)
        {
            Patient patient = await Patient.SelectPatient(id);
            if (patient == null) { patient = new Patient(); patient.IdPatient = await Patient.MaxIdPatient(); }
            return View(patient);
        }
        [HttpPost]
        public async Task<IActionResult> OnePatient(Patient patient)
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

        public async Task<IActionResult> IndicatorPatient(int id)
        {
            var list = await PatientIndicator.listPatIndicators(id);
            ViewBag.idCard = id;
            return View(list);
        }




        [HttpGet]
        public IActionResult OneIndicatorPatient(int id, string date)
        {
            var list = AnalysisModel.SelectIndicatorsValue(id, date);
            ViewBag.idCard = id;
            return View(list);
        }
        [HttpPost]
        public async Task<IActionResult> OneIndicatorPatient(List<AnalysisModel> patIndicator, int id)
        {
            if (ModelState.IsValid)
            {
                await PatientIndicator.SaveAnalysisAsync(patIndicator, id);
                return Redirect("~/NutritionistHome/MenuNutritionist");
            }
            ViewBag.idCard = id;
            return View(patIndicator);
        }
    }
}
