using PizzaApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApp.Console
{
    class Program
    {

        static void Main(string[] args)
        {
            const int AUTHORIZATION_CHOUSE = 1;
            const int REGISTRATION_CHOUSE = 2;
            int chouse;
            if (int.TryParse(System.Console.ReadLine(), out chouse))
            {
                if (chouse == AUTHORIZATION_CHOUSE)
                {
                    // Registration();
                }
                else if (chouse == REGISTRATION_CHOUSE)
                {

                }
            }
            System.Console.ReadLine();
        }


        public void Registration()
        {

            System.Console.WriteLine("Введите Логин");
            while (true)
            {
                string login = System.Console.ReadLine();
                if (!(login is null) && login.Length > 3)
                {
                    // Проверка на существование пользователя с таким же логином
                    break;
                }
                System.Console.WriteLine("Логин должен состоять как минимум из 3-х символов");
            }


            System.Console.WriteLine("Pass");
            string password = System.Console.ReadLine();

            System.Console.WriteLine("Phone num");
            string num = System.Console.ReadLine();

            TwillioService twillioService = new TwillioService();

            if (twillioService.SendMessage(num))
            {
                System.Console.WriteLine("true");
            }
            else
            {
                System.Console.WriteLine("false");
            }

        }


        public void Authorization()
        {
            System.Console.WriteLine("Введите логин");
            string login = System.Console.ReadLine();

            System.Console.WriteLine("Введите пароль");
            string password = System.Console.ReadLine();

            // Поиск в БД подобного пользователя
        }

    }
}
