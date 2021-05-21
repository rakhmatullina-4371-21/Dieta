using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Diet.Areas.Admin.Contollers
{
    public class AdminHomeController : Controller
    {
        [Area("Admin")]
        public IActionResult MenuAdmin()
        {
            return View();
        }
        public IActionResult Pat()
        {
            return View();
        }
    }
}
