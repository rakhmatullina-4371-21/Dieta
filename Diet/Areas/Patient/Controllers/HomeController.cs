using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Diet.Areas.Patient.Contollers
{
    public class HomeController : Controller
    {
        [Area("Patient")]
        public IActionResult Menu()
        {
            return View();
        }
    }
}
