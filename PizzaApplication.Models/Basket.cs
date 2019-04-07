using System.Collections.Generic;

namespace PizzaApp.Models
{
    public class Basket
    {
        public int Id { get; set; }
        public List<Pizza> Products { get; set; }
        public User User { get; set; }
    }
}
