using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Diet.Contollers
{
    public class HomeController : Controller
    {
        public IActionResult Start()
        {
            return View();
        }
    }
}
