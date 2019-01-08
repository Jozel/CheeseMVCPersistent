using Microsoft.AspNetCore.Mvc;
using CheeseMVC.Models;
using System.Collections.Generic;
using CheeseMVC.ViewModels;
using CheeseMVC.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;

namespace CheeseMVC.Controllers
{
    public class MenuController : Controller
    {
        private CheeseDbContext context;

        public MenuController(CheeseDbContext dbContext)
        {
            context = dbContext;
        }

        public IActionResult Index()
        {
            IList<Menu> menus = context.Menus.ToList();
            return View(menus);
        }

        public IActionResult Add()
        {
            AddMenuViewModel menu = new AddMenuViewModel();
            return View(menu);
        }

        [HttpPost]
        public IActionResult Add(AddMenuViewModel addMenuViewModel)
        {
            if (ModelState.IsValid)
            {
                Menu newMenu = new Menu
                {
                    Name = addMenuViewModel.Name
                };
                context.Menus.Add(newMenu);
                context.SaveChanges();
                return Redirect("/Menu/ViewMenu/" + newMenu.ID);
            }

            return View(addMenuViewModel);
        }

        public IActionResult ViewMenu(int id)
        {
            Menu menu = context.Menus.Single(m => m.ID == id);

            List<CheeseMenu> items = context
                .CheeseMenus
                .Include(item => item.Cheese)
                .Include(item => item.Cheese.Category)
                .Where(cm => cm.MenuID == id)
                .ToList();

            ViewMenuViewModel viewMenuViewModel = new ViewMenuViewModel
            {
                Menu = menu,
                Items = items
            };

            return View(viewMenuViewModel);
        }

        public IActionResult AddItem(int id)
        {
            Menu menu = context.Menus.Single(m => m.ID == id);
            IList<Cheese> cheeses = context.Cheeses.ToList();
            AddMenuItemViewModel addMenuItemViewModel = new AddMenuItemViewModel(menu, cheeses);

            return View(addMenuItemViewModel);
        }

        [HttpPost]
        public IActionResult AddItem(int CheeseID, int MenuID)
        {
            IList<CheeseMenu> existingItems = context.CheeseMenus
                    .Where(cm => cm.CheeseID == CheeseID)
                    .Where(cm => cm.MenuID == MenuID).ToList();

            if (existingItems.Count == 0)
            {
                Menu addedToMenu = context.Menus.Single(m => m.ID == MenuID);
                Cheese addedCheese = context.Cheeses.Single(c => c.ID == CheeseID);
                Console.WriteLine(addedCheese.Category.Name);
                CheeseMenu newCheeseMenu = new CheeseMenu
                {
                    MenuID = MenuID,
                    Menu = addedToMenu,
                    CheeseID = CheeseID,
                    Cheese = addedCheese,
                    Category = addedCheese.Category,

                };

                context.CheeseMenus.Add(newCheeseMenu);
                context.SaveChanges();

                return Redirect("/Menu/ViewMenu/" + MenuID); ;
            }
            Menu menu = context.Menus.Single(m => m.ID == MenuID);
            IList<Cheese> cheeses = context.Cheeses.ToList();
            AddMenuItemViewModel addMenuItemViewModel = new AddMenuItemViewModel(menu, cheeses);
            return View(addMenuItemViewModel);
        }
    }
}
