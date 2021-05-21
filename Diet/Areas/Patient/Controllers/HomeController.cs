using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Diet.Areas.Patient.Controllers
{
    public class HomeController : Controller
    {
        [Area("Patient")]
        public IActionResult MenuPatient()
        {
            return View();
        }
        public IActionResult Card()
        {
            return View();
        }
        public IActionResult Diary()
        {
            return View();
        }
    }
}
