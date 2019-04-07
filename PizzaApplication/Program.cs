using PizzaApp.Models;
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
            System.Console.WriteLine("Добро пожаловать в PizzaApplication!");
            System.Console.WriteLine("\nЕсли вы уже прошли регистрацию, введите 1.");
            System.Console.WriteLine("Если Вы хотите создать новый аккаунт введите 2.");
            const int AUTHORIZATION_CHOUSE = 1;
            const int REGISTRATION_CHOUSE = 2;
            int chouse;
            User user;
            while (true)
            {
                if (int.TryParse(System.Console.ReadLine(), out chouse))
                {
                    if (chouse == AUTHORIZATION_CHOUSE)
                    {
                        user = Authorization();
                        break;
                    }
                    else if (chouse == REGISTRATION_CHOUSE)
                    {
                        user = Registration();
                        break;
                    }
                    System.Console.WriteLine("Есть только 2 варианта ответа!");
                    continue;
                }
                System.Console.WriteLine("Вводите данные нормально!");
            }

            while (true)
            {
                System.Console.Clear();
                System.Console.WriteLine($"\t{user.FullName} Денег на счету:{user.Money}");
                System.Console.WriteLine("Главное меню.");
                System.Console.WriteLine("\n(1)-Добавить пиццу в корзину.");
                System.Console.WriteLine("(2)-Посмотреть корзину");
                System.Console.WriteLine("(3)-Пополнение счета.");
                System.Console.WriteLine("\n(4)-Купить");
                System.Console.WriteLine("\n(5)-Выход");

                while (true)
                {
                    if (int.TryParse(System.Console.ReadLine(), out chouse) && chouse > 0 && chouse < 5)
                    {
                        switch (chouse)
                        {
                            case 1:
                                // Вывод всех пицц и ввод ИД пиццы для покупки
                                break;
                            case 2:
                                // Вывод корзины
                                break;
                            case 3:
                                System.Console.Clear();
                                System.Console.WriteLine("Вас приветствует банк HalyavaBank");
                                System.Console.WriteLine("Введите количество денег которое хотите получить");
                                while (true)
                                {
                                    if (int.TryParse(System.Console.ReadLine(), out int amount) && amount > 0)
                                    {
                                        if (amount < 30000)
                                        {
                                            user.Money += amount;
                                            System.Console.WriteLine("Средства успешно получены!(Enter - для выхода в меню)\n");
                                            break;
                                        }
                                        System.Console.WriteLine("Уважаемый, не наглейте, пожалуйста!");
                                        continue;
                                    }
                                    System.Console.WriteLine("Такого числа не существует!");
                                }

                                break;
                            case 4:
                                break;
                            case 5:
                                return;
                        }
                        System.Console.Read();
                        break;
                    }
                    System.Console.WriteLine("Неверный ввод!");
                }

            }
        }


        public static User Registration()
        {
            string login;
            System.Console.WriteLine("Введите логин");
            while (true)
            {
                login = System.Console.ReadLine();
                if (!(login is null) && login.Length >= 3)
                {
                    // Проверка на существование пользователя с таким же логином
                    break;
                }
                System.Console.WriteLine("Логин должен состоять как минимум из 3-х символов");
            }

            string name;
            System.Console.WriteLine("Введите Ваше имя");
            while (true)
            {
                name = System.Console.ReadLine();
                if (!(name is null) && name.Length > 1)
                    break;

                System.Console.WriteLine("Введите Ваше настоящее имя!");
            }

            string password;
            System.Console.WriteLine("Введите пароль");
            while (true)
            {
                password = System.Console.ReadLine();
                if (!(password is null) && password.Length >= 5)
                    break;
                else
                    System.Console.WriteLine("\nПароль должен состоять как минимум из 5 символов!");
            }


            TwillioService twillioService = new TwillioService();
            System.Console.WriteLine("Введите номер телефона");
            string number;
            while (true)
            {
                number = System.Console.ReadLine();

                if (!twillioService.SendMessage(number))
                {
                    System.Console.WriteLine("Ошибка отправки СМС!");
                    System.Console.WriteLine("Проверьте, правильно ли вы ввели данные.");
                    System.Console.WriteLine("\nПожалуйста, введите Ваш номер телефона в формате +XYYYXXXYYXX");
                }
                else
                    break;
            }
            System.Console.WriteLine("СМС с кодом успешно отправлено!");
            System.Console.WriteLine("Дождитесь прихода сообщения и введите код подтверждения!");

            const int AMOUNT_SYMBOLS_INT_CODE = 4;
            while (true)
            {
                if (int.TryParse(System.Console.ReadLine(), out int code) && code.ToString().Length == AMOUNT_SYMBOLS_INT_CODE)
                {
                    if (twillioService.IsRightCode(code))
                        break;

                    System.Console.WriteLine("Код неверный! Убедитесь, что Вы ввели код правильно!");
                    System.Console.WriteLine("Введите еще раз!");
                    continue;
                }
                System.Console.WriteLine("Это даже не похоже на код! Код состоит из 4-х цифр");
                System.Console.WriteLine("Введите еще раз!");
            }

            System.Console.WriteLine("Отлично, вы прошли процедуру регистрации!");

            return new User
            {
                Login = login,
                FullName = name,
                Password = password,
                PhoneNumber = number,
            };

        }


        public static User Authorization()
        {
            System.Console.WriteLine("Введите логин");
            string login = System.Console.ReadLine();

            System.Console.WriteLine("Введите пароль");
            string password = System.Console.ReadLine();

            // Поиск в БД подобного пользователя
            return new User();
        }

    }
}
