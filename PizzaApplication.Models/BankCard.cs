using System;

namespace PizzaApp.Models
{
    public class BankCard
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public string CardHolderName { get; set; }
        public int CVV { get; set; }
        public DateTime Validity { get; set; }
        public int MoneyAmount { get; set; }
    }
}