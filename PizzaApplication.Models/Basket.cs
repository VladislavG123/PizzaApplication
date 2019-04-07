using System.Collections.Generic;

namespace PizzaApp.Models
{
    public class Basket
    {
        public int Id { get; set; }
        public List<int> PizzaId { get; set; }  
        public int UserId { get; set; }
    }
}