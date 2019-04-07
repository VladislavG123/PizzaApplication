using System;

namespace PizzaApp.Models
{
    public class BankCard
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public int Cvc { get; set; }
        public DateTime Validity { get; set; }
    }
}