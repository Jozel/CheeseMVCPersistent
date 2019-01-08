using CheeseMVC.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CheeseMVC.ViewModels
{
    public class EditCheeseViewModel : AddCheeseViewModel
    {
        [Required]
        public int CheeseID { get; set; }

        public EditCheeseViewModel(IEnumerable<CheeseCategory> categories)
        {
            Categories = new List<SelectListItem>();
            foreach (CheeseCategory cat in categories)
            {
                Categories.Add(new SelectListItem
                {
                    Value = cat.ID.ToString(),
                    Text = cat.Name
                });
            }

        }
        public EditCheeseViewModel()
        {
        }
    }
}
