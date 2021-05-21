using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Diet.Areas.Admin.Contollers
{
    public class HomeController : Controller
    {
        [Area("Admin")]
        public IActionResult Menu()
        {
            return View();
        }
        public IActionResult PatientSelect()
        {
            return View();
        }
    }
}
