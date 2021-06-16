using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Diet.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Diet.Models.ViewModels;

namespace Diet.PatientHome.Controllers
{

    [Authorize]
    public class PatientHomeController : Controller
    {
        public async Task<IActionResult> MenuPatient()
        {
            return View(await Patient.SelectPatient(Convert.ToInt32(HttpContext.User.Identity.Name)));
        }
        [HttpGet]
        public IActionResult Card(int id)
        {
            DietDBContext db = new DietDBContext();
            ViewData["employee"] = db.Employees.Where(p => p.IdPositionNavigation.Position1 == "Диетолог").Select(r => new SelectListItem
            {
                Text =$"{r.Surname} {r.Name} {r.Lastname}",
                Value = r.IdPosition.ToString()
            });
            PatientCard card;
            if (db.PatientCards.Where(p => p.IdPatient == id && p.Activ == true).Count() != 0)
            {
                 card= db.PatientCards.OrderBy(p=>p.IdCard).Select(p=>p).Where(p => p.IdPatient == id && p.Activ == true).Last();
            }
            else card = new PatientCard();
            
            return View(card);
        }
        [HttpPost]
        public async Task<IActionResult> Card(PatientCard card)          //Сохрание карты пациента
        {
            if (ModelState.IsValid)
            {
                await PatientCard.SavePatCard(int.Parse(HttpContext.User.Identity.Name), card);
                return Redirect("~/PatientHome/MenuPatient");
            }
            return View();
        }
        public async Task<IActionResult> Diary()
        {
           var model= await DiaryModel.listMeal(int.Parse(HttpContext.User.Identity.Name));
            return View(model);
        }
        public async Task<IActionResult> Menu(int id)
        {
            var model = await DiaryModel.selectMenu(id);
            return View(model);
        }
    }
}
