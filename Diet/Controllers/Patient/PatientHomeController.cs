using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Diet.Areas.Patient.Controllers
{

    [Authorize]
    public class PatientHomeController : Controller
    {
        [Area("Patient")]
        public IActionResult MenuPatient( int id)
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
