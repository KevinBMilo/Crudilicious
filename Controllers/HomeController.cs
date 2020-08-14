using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Crudlicious.Models;
using Microsoft.EntityFrameworkCore;

namespace Crudlicious.Controllers
{
    public class HomeController : Controller
    {
        private MyContext dbContext;
        public HomeController(MyContext context)
        {
            dbContext = context;
        }
     
        [HttpGet("")]
        public IActionResult Index()
        {
            List<Dish> AllDishes = dbContext.Dishes
    			.OrderByDescending(u => u.CreatedAt)
    			.ToList();
			return View("Index", AllDishes);
        }
        
        [HttpGet("new")]
        public IActionResult NewDish()
        {
            return View("New");
        }

        [HttpGet("{DishId}")]
        public IActionResult Show(int DishId)
        {
            Dish oneDish = dbContext.Dishes.FirstOrDefault(dish => dish.DishId == DishId);
            return View("Show", oneDish);
        }

        [HttpGet("edit/{DishId}")]
        public IActionResult Edit(int DishId)
        {
            Dish RetrievedDish = dbContext.Dishes.FirstOrDefault(Dish => Dish.DishId == DishId);
            return View("Update", RetrievedDish);
        }

        [HttpPost("create")]
        public IActionResult Create(Dish newDish)
        {
            Dish submittedDish = newDish;
            if(ModelState.IsValid)
            {
                dbContext.Add(newDish);
                dbContext.SaveChanges();
                return RedirectToAction("Index", newDish);
            }
            return View("New");
        }

        [HttpPost("change/{DishId}")]
        public IActionResult Change(int DishId, Dish formDish)
        {
            Dish RetrievedDish = dbContext.Dishes.FirstOrDefault(Dish => Dish.DishId == DishId);
            if(ModelState.IsValid)
            {
                RetrievedDish.Dishname = formDish.Dishname;
                RetrievedDish.Chef = formDish.Chef;
                RetrievedDish.Description = formDish.Description;
                RetrievedDish.Tastiness = formDish.Tastiness;
                RetrievedDish.Calories = formDish.Calories;
                RetrievedDish.UpdatedAt = DateTime.Now;
                dbContext.SaveChanges();
                return RedirectToAction("Show", new {DishId = DishId});
            }
            return View("Update", RetrievedDish);
        }

        [HttpGet("delete/{DishId}")]
        public IActionResult Delete(int DishId)
        {
            Dish RetrievedDish = dbContext.Dishes.FirstOrDefault(Dish => Dish.DishId == DishId);
            dbContext.Dishes.Remove(RetrievedDish);
            dbContext.SaveChanges();
            return RedirectToAction ("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
