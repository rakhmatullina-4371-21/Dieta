using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Diet.Areas.Start.Contollers
{
    public class HomeController : Controller
    {
        [Area("Start")]
        public IActionResult Start()
        {
            return View();
        }
    }
}
