using System;

namespace PizzaApp.Models
{
    public class BankCard
    {
        public string Number { get; set; }
        public int CVC { get; set; }
        public DateTime Validity { get; set; }
    }
}