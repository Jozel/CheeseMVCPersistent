using Microsoft.AspNetCore.Mvc;
using CheeseMVC.Models;
using System.Collections.Generic;
using CheeseMVC.ViewModels;
using CheeseMVC.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace CheeseMVC.Controllers
{
    public class CheeseController : Controller
    {
        private CheeseDbContext context;

        public CheeseController(CheeseDbContext dbContext)
        {
            context = dbContext;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            IList<Cheese> cheeses = context.Cheeses.Include(c => c.Category).ToList();
            return View(cheeses);
        }

        public IActionResult Add()
        {
            AddCheeseViewModel addCheeseViewModel = new AddCheeseViewModel(context.Categories.ToList());
            return View(addCheeseViewModel);
        }

        [HttpPost]
        public IActionResult Add(AddCheeseViewModel addCheeseViewModel)
        {
            CheeseCategory category = context.Categories.Single(c => c.ID == addCheeseViewModel.CategoryID);

            if (ModelState.IsValid && category != null)
            {
                // Add the new cheese to my existing cheeses

                Cheese newCheese = new Cheese
                {
                    Name = addCheeseViewModel.Name,
                    Description = addCheeseViewModel.Description,
                    Category = category,
                    CategoryID = addCheeseViewModel.CategoryID
                };

                context.Cheeses.Add(newCheese);
                context.SaveChanges();

                return Redirect("/Cheese");
            }

            return View(addCheeseViewModel);
        }

        public IActionResult Remove()
        {
            ViewBag.title = "Remove Cheeses";
            ViewBag.cheeses = context.Cheeses.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Remove(int[] cheeseIds)
        {
            foreach (int cheeseId in cheeseIds)
            {
                Cheese theCheese = context.Cheeses.Single(c => c.ID == cheeseId);
                context.Cheeses.Remove(theCheese);
            }

            context.SaveChanges();

            return Redirect("/");
        }

        public IActionResult Edit(int cheeseID)
        {
            ViewBag.title = "Edit Cheese";
            Cheese theCheese = context.Cheeses.Single(c => c.ID == cheeseID);
            EditCheeseViewModel editCheeseVM = new EditCheeseViewModel(context.Categories.ToList())
            {
                CheeseID = theCheese.ID,
                Name = theCheese.Name,
                Description = theCheese.Description,
                CategoryID = theCheese.CategoryID
            };
            return View(editCheeseVM);
        }

        [HttpPost]
        public IActionResult Edit(EditCheeseViewModel editCheeseVM)
        {
            if (ModelState.IsValid)
            {
                Cheese theCheese = context.Cheeses.Single(c => c.ID == editCheeseVM.CheeseID);
                CheeseCategory category = context.Categories.Single(c => c.ID == editCheeseVM.CategoryID);

                theCheese.Name = editCheeseVM.Name;
                theCheese.Description = editCheeseVM.Description;
                theCheese.CategoryID = editCheeseVM.CategoryID;
                theCheese.Category = category;

                context.SaveChanges();

                return Redirect("/");
            }

            return View(editCheeseVM);
        }
    }
}
