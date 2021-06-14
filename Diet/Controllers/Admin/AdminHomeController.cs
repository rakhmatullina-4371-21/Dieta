using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Diet.Models;
using Diet.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Diet
{

    [Authorize]
    public class AdminHomeController : Controller
    {
        DietDBContext db = new DietDBContext();
        Employee employee;
        public async Task<IActionResult> MenuAdmin()
        {
            employee = new Employee();
            var id =  HttpContext.User.Identity.Name;
            employee = await employee.ReturnEmp(id);
            return View(employee);
        }
        public IActionResult PatSelect()
        {
            return View(Patient.SelectPatients().OrderBy(p=>p.Surname));
        }

        public IActionResult EmpSelect()
        {
            return View(Employee.SelectEmployees(HttpContext.User.Identity.Name).OrderBy(p=>p.Surname));
        }

        public IActionResult DishSelect()
        {
            return View(Dish.SelectDishes());
        }

        public IActionResult ProdInDish(int id)
        {
            return View(DishesProduct.SelectDishProd(id));
        }
        [HttpGet]
        public async Task<IActionResult> OnePatient(int id)
        {
            Patient patient = await Patient.SelectPatient(id);
            if (patient==null) { patient = new Patient(); patient.IdPatient = await Patient.MaxIdPatient(); }
            return View(patient);
        }
        [HttpPost]
        public async Task<IActionResult> OnePatient(Patient patient)
        {
            if (ModelState.IsValid && await Patient.LoginContains(patient.Login, patient.IdPatient))
            {
                await  patient.SavePatient(patient);
                return Redirect("~/AdminHome/MenuAdmin");
            }
            else
            {
                ModelState.AddModelError("", "Такой логин уже существует");
                return View(patient);
            }
        }


        public async Task<IActionResult> DeleteOnePatient(Patient patient)
        {
            if (ModelState.IsValid)
            {
                await patient.DeletePatient(patient);
                return Redirect("~/AdminHome/PatSelect");
            }
            return View(patient);
        }


        [HttpGet]
        public async Task<IActionResult> OneEmployee(int id)
        {
            Employee employee = await Employee.SelectEmployee(id);
            ViewData["position"] = db.Positions.Select(r => new SelectListItem
            {
                Text =r.Position1,
                Value = r.IdPosition.ToString() 
            });
            if (employee == null) { employee = new Employee(); employee.IdEmployee = await Employee.MaxIdEmployee(); }
            return View(employee);
        }
        [HttpPost]
        public async Task<IActionResult> OneEmployee(Employee employee)
        {
            ViewData["position"] = db.Positions.Select(r => new SelectListItem
            {
                Text = r.Position1,
                Value = r.IdPosition.ToString()
            });
            if (ModelState.IsValid && await Employee.LoginContains(employee.Login, employee.IdEmployee))
            {
                await employee.SaveEmployee(employee);
                return Redirect("~/AdminHome/MenuAdmin");
            }
            else
            {
                ModelState.AddModelError("", "Такой логин уже существует");
                return View(employee);
            }
        }

        
        public async Task<IActionResult> DeleteOneEmployee(Employee employee)
        {
            if (ModelState.IsValid)
            {
                await employee.DeleteEmployee(employee);
                return Redirect("~/AdminHome/EmpSelect");
            }
            return View(employee);
        }

        [HttpGet]
        public async Task<IActionResult> OneDish(int id)
        {
            ListProductsInDishModel model =await ListProductsInDishModel.ListInProd(id);
            return View(model);
        }
        [HttpPost]
        public  IActionResult OneDish(ListProductsInDishModel model)
        {
            

            if (ModelState.IsValid)
            {
                ListProductsInDishModel.SaveDish(model);
                Dish d = new Dish();
                d = model.OneDish;
                ListProductsInDishModel.SaveProductDish(model, d);
                return Redirect("~/AdminHome/MenuAdmin");
            }
            ModelState.AddModelError("", "Некорректные данные ");
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> OneProduct(int id)
        {
            ViewData["diag"] = db.Diagnoses.Select(r => new SelectListItem
            {
                Text = r.IdDiagnosis.ToString(),
                Value = false.ToString()
            });
            Product product = await Product.SelectOneProd(id);
            if (product == null) { product = new Product(); product.IdProduct = await Product.MaxIdProd(); }
           AddProductModel model= await  AddProductModel.ProductInDish(product.IdProduct);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> OneProduct(AddProductModel product)
        {
            if (ModelState.IsValid)
            {
                await Product.SaveProd(product);
                return Redirect("~/AdminHome/MenuAdmin");
            }
            return View(product);
        }
        public async Task<IActionResult> DeleteOneDish(ListProductsInDishModel model)
        {
            if (ModelState.IsValid)
            {

                await Dish.DelDish(new Dish {IdDish=model.OneDish.IdDish });
                return Redirect("~/AdminHome/MenuAdmin");
            }
            return View(model);
        }
        
        public async Task<IActionResult> DeleteProduct(int id)
        {

            await Product.DelProd(id);
            return Redirect("~/AdminHome/MenuAdmin");

        }

    }
}
