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
    //[Area("Admin")]
    //[Route("[Admin]/[AdminHome]/[Actions]/{page:int?}")]
    [Authorize]
    public class AdminHomeController : Controller
    {
        DietDBContext db = new DietDBContext();
        Employee employee;
        public async Task<IActionResult> MenuAdmin(string returnUrl)
        {
            //    string ID = Request.QueryString["recordID"];
            employee = new Employee();

            var id =  HttpContext.User.Identity.Name;
            employee = await employee.ReturnEmp(id);
            return View(employee);
        }
        public IActionResult PatSelect()
        {
            var list = db.Patients.Select(p => p).ToList();
            return View(list);
        }
        public async Task<IActionResult> OnePatient(int? id)
        {
           Patient patient = await db.Patients.FirstOrDefaultAsync(p => p.IdPatient==id);
            return View(patient);
        }
    }
}
