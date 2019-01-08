using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheeseMVC.Models
{
    public class CheeseMenu
    {
        public int MenuID { get; set; }
        public Menu Menu { get; set; } = new Menu();
        public int CheeseID { get; set; }
        public Cheese Cheese { get; set; } = new Cheese();
        public CheeseCategory Category { get; set; } = new CheeseCategory();
    }
}
