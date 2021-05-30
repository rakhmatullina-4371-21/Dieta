using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Diet.Models;
using Microsoft.AspNetCore.Mvc;

namespace Diet.Controllers.Nutritionist
{
    public class NutritionistHomeController : Controller
    {

        public IActionResult CardOnePat( int idPat)
        {
            return View(PatientCard.SelectPatientsNutr(idPat));
        }
        public IActionResult MenuNutritionist()
        { 
            var id = HttpContext.User.Identity.Name;
            return View(PatientCard.SelectPatientsNutr(int.Parse(id)).Distinct());
        }
        public IActionResult PatSelect()
        {
            return View(Patient.SelectPatientCardNull(int.Parse(HttpContext.User.Identity.Name)));
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
                return Redirect("~/AdminHome/PatSelect");
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

                await patient.SavePatient(patient);
                return CardPatient(patient.IdPatient);
            }
            return View(patient);
        }

    }
}
