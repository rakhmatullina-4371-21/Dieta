using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Diet.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Diet.Models;
using Diet.Areas;

namespace Diet.Controllers
{
    public class AccountController : Controller
    {
        private DietDBContext db;
        public AccountController(DietDBContext db)
        {
            this.db = db;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model, string returnUrl)
        {
                    string  password = PasswordHash.GetHash(model.Password);    //Шифрование введенного пароля
                    Patient patient = await db.Patients.FirstOrDefaultAsync(u => u.Login == model.Email && u.Password == password);
                    if (patient != null)
                    {
                        await Authenticate(patient.Login); // аутентификация
                          return Redirect(returnUrl ?? "/");
                       //return RedirectToAction("MenuPatient", "PatientStart", patient.IdPatient);
                    }
                    else
                    {
                       Employee emp = await db.Employees.FirstOrDefaultAsync(u => u.Login == model.Email && u.Password == password);
                        if (emp != null)
                        {

                            await Authenticate(model.Email); // аутентификация
                            if (await db.Employees.FirstOrDefaultAsync(p => p.IdPosition ==db.Positions.FirstOrDefault(o => o.Position1 == "Администратор").IdPosition) != null)
                            {
                                 //return RedirectToPage("Ident", "AdminHome", emp);
                                await Authenticate(emp.IdEmployee.ToString()); // аутентификация
                                return Redirect(returnUrl ?? "AdminHome/MenuAdmin/id?="+emp.IdEmployee);
                            }

                        }
                        ModelState.AddModelError("", "Некорректные логин и(или) пароль");
                    }

            return View(model);
        }

        private async Task Authenticate(string user)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType,user)
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}

