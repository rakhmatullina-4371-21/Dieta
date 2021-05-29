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
        DietDBContext db = new DietDBContext();
        Employee employee;
        public async Task<IActionResult> MenuAdmin()
        {
            employee = new Employee();
            var id = HttpContext.User.Identity.Name;
            employee = await employee.ReturnEmp(id);
            return View(employee);
        }
        public IActionResult MenuNutritionist() 
        {
            employee = new Employee();
            var id = HttpContext.User.Identity.Name;
            return View(Patient.SelectPatients(int.Parse(id)));
        }
        public IActionResult PatSelect()
        {
            return View(Patient.SelectPatientCardNull());
        }

        [HttpGet]
        public async Task<IActionResult> CardPatient(int id)
        {
            Patient patient = await Patient.SelectPatient(id);
            if (patient == null) { patient = new Patient(); patient.IdPatient = await Patient.MaxIdPatient(); }
            return View(patient);
        }
        [HttpPost]
        public async Task<IActionResult> CardPatient(PatientCard card)
        {
            if (ModelState.IsValid)
            {

                return Redirect("~/AdminHome/PatSelect");
            }
            return View();
        }

    }
}
