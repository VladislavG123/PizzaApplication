using PizzaApp.Models;
using PizzaApp.Services;
using PizzaApplication.DataAccess;
using System;
using System.Collections.Generic;

namespace PizzaApp.Console
{
    class Program
    {
        static UsersTableService usersTableService = new UsersTableService();
        static BasketsTableService basketsTableService = new BasketsTableService();
        static BasketsAndPizzasTableService basketsAndPizzasTableService = new BasketsAndPizzasTableService();
        static PizzasTableService pizzasTableService = new PizzasTableService();

        static void Main(string[] args)
        {
            System.Console.WriteLine("Добро пожаловать в PizzaApplication!");
            System.Console.WriteLine("\nЕсли вы уже прошли регистрацию, введите 1.");
            System.Console.WriteLine("Если Вы хотите создать новый аккаунт введите 2.");
            System.Console.WriteLine("Если Вы хотите создать новый аккаунт через телеграм введите 3.");
            const int AUTHORIZATION_CHOUSE = 1;
            const int REGISTRATION_CHOUSE = 2;
            const int TELEGRAM_CHOUSE = 3;
            int chouse;
            User user;
            while (true)
            {
                if (int.TryParse(System.Console.ReadLine(), out chouse))
                {
                    if (chouse == AUTHORIZATION_CHOUSE)
                    {
                        try
                        {
                            user = Authorization();
                        }
                        catch (Exception exception)
                        {
                            System.Console.WriteLine("Введите еще раз!");
                            System.Console.WriteLine(exception.Message);
                            continue;
                        }
                        break;
                    }
                    else if (chouse == REGISTRATION_CHOUSE)
                    {
                        user = Registration();
                        break;
                    }
                    else if (chouse == TELEGRAM_CHOUSE)
                    {
                        while (true)
                        {
                            System.Console.WriteLine("Напишите нашему телеграм боту PizzaAppBot");
                            TelegramService telegram = new TelegramService();

                            user = telegram.Start();
                            System.Console.WriteLine("После регистрации введите Enter");
                            System.Console.Read();
                            if (user.Login is null || user.FullName is null || user.Password is null || user.PhoneNumber is null)
                            {
                                System.Console.WriteLine("Данные введены неверно!\nНапишите нашему боту еще раз!");
                            }
                            else
                                break;

                        }
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
                System.Console.WriteLine($"\t{user.FullName}, Денег на счету:{user.Money}");
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
                                foreach (var pizza in pizzasTableService.SelectPizzas())
                                {
                                    System.Console.WriteLine($"Номер - {pizza.Id}");
                                    System.Console.WriteLine($"Название - {pizza.Name}");
                                    System.Console.WriteLine($"Ингредиенты - {pizza.Description}");
                                    System.Console.WriteLine($"Цена - {pizza.Cost}");
                                    System.Console.WriteLine($"Размер - {pizza.Size}");
                                    System.Console.WriteLine();
                                }
                                int pizzaNumber = 0;
                                while (true)
                                {
                                    System.Console.WriteLine("Введите номер пиццы");
                                    if (int.TryParse(System.Console.ReadLine(), out int result))
                                    {
                                        foreach (var pizza in pizzasTableService.SelectPizzas())
                                        {
                                            if (pizza.Id == result)
                                            {
                                                pizzaNumber = result;
                                                break;
                                            }
                                        }
                                        if (pizzaNumber == 0)
                                        {
                                            System.Console.WriteLine("Пиццы с таким номером нет");
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        System.Console.WriteLine("Некорректный номер");
                                    }
                                }

                                foreach (var basket in basketsTableService.SelectBaskets())
                                {
                                    if (basket.UserId == user.Id)
                                    {
                                        basketsAndPizzasTableService.InsertValues(basket.Id, pizzaNumber);
                                    }
                                }
                                break;
                            case 2:
                                List<Basket> baskets = GetBaskets();
                                int basketId = 0;
                                foreach (var basket in baskets)
                                {
                                    if (basket.UserId == user.Id)
                                    {
                                        basketId = basket.Id;
                                    }
                                }
                                if (baskets.Count != 0)
                                {
                                    List<int[]> values = basketsAndPizzasTableService.SelectValues();
                                    System.Console.Clear();
                                    System.Console.WriteLine("Корзина:");
                                    foreach (var value in values)
                                    {
                                        if (basketId == value[0])
                                        {
                                            foreach (var pizza in pizzasTableService.SelectPizzas())
                                            {
                                                if (value[1] == pizza.Id)
                                                {
                                                    System.Console.WriteLine($"Название - {pizza.Name}");
                                                    System.Console.WriteLine($"Ингредиенты - {pizza.Description}");
                                                    System.Console.WriteLine($"Цена - {pizza.Cost}");
                                                    System.Console.WriteLine($"Размер - {pizza.Size}");
                                                    System.Console.WriteLine();
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    System.Console.WriteLine("Корзина пуста");
                                }
                                System.Console.WriteLine("Нажмите Enter для продолжения");
                                break;
                            case 3:
                                System.Console.Clear();
                                System.Console.WriteLine("Вас приветствует банк HalyavaBank");
                                System.Console.WriteLine("Введите количество денег которое хотите получить(не более 30000)");
                                while (true)
                                {
                                    if (int.TryParse(System.Console.ReadLine(), out int amount) && amount > 0)
                                    {
                                        if (amount < 30000)
                                        {
                                            user.Money += amount;
                                            usersTableService.UpdateUser(user.Id, user.Money);
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
                                int cost = 0;
                                foreach (var pizza in pizzasTableService.SelectPizzas())
                                {
                                    cost += pizza.Cost;
                                }
                                if (cost > 0)
                                {
                                    if (cost <= user.Money)
                                    {
<<<<<<< HEAD
                                        user.Money -= cost;
                                        usersTableService.UpdateUser(user.Id, user.Money);
                                        System.Console.WriteLine($"Успешно оплачено! На вашем счету {user.Money}");
                                    }
                                    else
                                    {
                                        System.Console.WriteLine("Не достаточно средств для списания");
=======
                                        if (chouse == 1)
                                        {

                                        }
                                        else if (chouse == 2)
                                        {

                                        }
>>>>>>> 41297470d4f9b8172eef3dcf13c679bf56a38472
                                    }
                                }
                                else
                                {
                                    System.Console.WriteLine("Ваша корзина пуста");
                                }
                                System.Console.WriteLine("Нажмите Enter для продолжения");
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
            User user = new User();
            string login;
            System.Console.WriteLine("Введите логин");
            while (true)
            {
                login = System.Console.ReadLine();
                if (!(login is null) && login.Length >= 3)
                {
                    bool isExist = false;
                    foreach (var logins in usersTableService.SelectUsers())
                    {
                        if (login == logins.Login)
                        {
                            isExist = true;
                        }
                    }
                    if (!isExist)
                    {
                        break;
                    }
                    else
                    {
                        System.Console.WriteLine("Пользователь с таким логином уже существует");
                    }
                }
                else
                {
                    System.Console.WriteLine("Логин должен состоять как минимум из 3-х символов");
                }
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

            user = new User
            {
                Login = login,
                FullName = name,
                Password = password,
                PhoneNumber = number,
                Money = 0
            };
            usersTableService.InsertUser(user);
            int userId;
            foreach (var obj in usersTableService.SelectUsers())
            {
                if (obj.Login == user.Login && obj.Password == user.Password)
                {
                    userId = obj.Id;
                    basketsTableService.InsertBasket(new Basket { UserId = userId });
                }
            }
            return user;
        }
        public static User Authorization()
        {
            System.Console.WriteLine("Введите логин");
            string login = System.Console.ReadLine();

            System.Console.WriteLine("Введите пароль");
            string password = System.Console.ReadLine();

            List<User> users = usersTableService.SelectUsers();
            foreach (var user in users)
            {
                if (user.Login == login && user.Password == password)
                {
                    return user;
                }
            }
            throw new Exception("Пользователь не найден");
        }

        public static List<Basket> GetBaskets()
        {
            return basketsTableService.SelectBaskets();
        }
    }
}