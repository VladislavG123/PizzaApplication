﻿namespace PizzaApp.Models
{
    public class User
    {
        int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public BankCard Card { get; set; }
    }
}