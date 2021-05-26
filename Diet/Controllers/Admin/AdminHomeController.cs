using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Diet.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            return View(Employee.SelectEmployees(HttpContext.User.Identity.Name));
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






        [HttpGet]
        public async Task<IActionResult> OneEmployee(int id)
        {
            Employee employee = await Employee.SelectEmployee(id);
            ViewData["position"] = db.Positions.Select(r => new SelectListItem
            {
                Text =r.Position1,
                Value = r.IdPosition.ToString() 
            });
            if (employee == null) { employee = new Employee(); employee.IdEmployee = await Employee.MaxIdEmployee(); }
            return View(employee);
        }
        [HttpPost]
        public async Task<IActionResult> OneEmployee(Employee employee)
        {
            ViewData["position"] = db.Positions.Select(r => new SelectListItem
            {
                Text = r.Position1,
                Value = r.IdPosition.ToString()
            });
            if (ModelState.IsValid)
            {
                
                await employee.SaveEmployee(employee);
                return Redirect("~/AdminHome/EmpSelect");
            }
            return View(employee);
        }





    }
}
