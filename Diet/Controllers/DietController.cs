using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Diet.Models.ViewModels;

namespace Diet.Controllers
{
    public class DietController : Controller
    {
        public IActionResult Start()
        {
            return View();
        }
    }
}
