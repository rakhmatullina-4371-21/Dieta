using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Diet.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Diet
{

    [Authorize]
    public class AdminHomeController : Controller
    {
        DietDBContext db = new DietDBContext();
        Employee employee;
        public async Task<IActionResult> MenuAdmin(string returnUrl)
        {
            employee = new Employee();
            var id =  HttpContext.User.Identity.Name;
            employee = await employee.ReturnEmp(id);
            return View(employee);
        }
        public IActionResult PatSelect()
        {
            return View(Patient.SelectPatients());
        }

        public IActionResult EmpSelect()
        {
            return View(Employee.SelectEmployees());
        }


        [HttpGet]
        public async Task<IActionResult> OnePatient(int id)
        {
            Patient patient = await Patient.SelectPatient(id);
            if (patient==null) { patient = new Patient(); patient.IdPatient = await Patient.MaxIdPatient(); }
            return View(patient);
        }
        [HttpPost]
        public async Task<IActionResult> OnePatient(Patient patient)
        {
            if (ModelState.IsValid)
            {

                await  patient.SavePatient(patient);
                return Redirect("~/AdminHome/PatSelect");
            }
            return View(patient);
        }

        public async Task<IActionResult> AddPatient(Patient patient)
        {

            patient = await db.Patients.FirstOrDefaultAsync(p => p.IdPatient == patient.IdPatient);
            return View();
        }


        public IActionResult EmpSelect()
        {
            return View(Patient.SelectPatients());
        }




    }
}
