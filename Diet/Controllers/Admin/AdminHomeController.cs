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
            return View(Patient.SelectPatients());
        }

        public IActionResult EmpSelect()
        {
            return View(Employee.SelectEmployees(HttpContext.User.Identity.Name));
        }

        public IActionResult DishSelect()
        {
            return View(Dish.SelectDishes());
        }

        public IActionResult ProdInDish(int id)
        {
            return View(DishesProduct.SelectDishProd(id));
        }




        public IActionResult SaveProdInDish(IEnumerable<Product> product, int idDish)
        {
           
            return View(Product.SelectProducts());
        }
 
        public IActionResult ProdSelect(IEnumerable<Product> product,int idDish)
        {
            var t = product;
            ViewBag.id = idDish;
            return View(Product.SelectProducts());
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
            if (ModelState.IsValid)
            {

                await  patient.SavePatient(patient);
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
            if (ModelState.IsValid)
            {
                
                await employee.SaveEmployee(employee);
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
                return Redirect("~/AdminHome/DishSelect");
            }
            ModelState.AddModelError("", "Некорректные данные ");
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> OneProduct(int id)
        {

            Product product = await Product.SelectOneProd(id);
            if (product == null) { product = new Product(); product.IdProduct = await Product.MaxIdProd(); }
            return View(product);
        }
        [HttpPost]
        public async Task<IActionResult> OneProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                await product.SaveProd(product);
                return Redirect("~/AdminHome/");
            }
            return View(product);
        }

        public async Task<IActionResult> DeleteProduct(int id)
        {

            await Product.DelProd(id);
            return Redirect("~/AdminHome/DishSelect");

        }

    }
}
