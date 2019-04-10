using PizzaApp.Services.Abstract;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Args;
using System.Text.RegularExpressions;
using PizzaApp.Models;
using PizzaApplication.DataAccess;

namespace PizzaApp.Services
{
    public class TelegramService
    {
        public static TelegramBotClient TelegramBot { get; set; }
        readonly string _token;
        public static User User { get; set; } = new User();

        public TelegramService()
        {

            _token = "687100641:AAGsutNjTbKPXtIvrKM949hecqyM0o_Q0JM";
            TelegramBot = new TelegramBotClient(_token);
        }
        public User GetUser()
        {
            return User;
        }
        public User Start()
        {
            TelegramBot.OnMessage += BotOnMessageReceived;
            TelegramBot.StartReceiving(new UpdateType[] { UpdateType.Message });
            return User;
        }

        private static async void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            Telegram.Bot.Types.Message message = messageEventArgs.Message;
            if (message == null || message.Type != MessageType.Text) return;

            string answer = "";

            if (message.Text == "/help" || message.Text == "/start")
            {
                answer = "Добро пожаловать в PizzaApplication!\nВводите данные в формате: \n/логин-[ЛОГИН] \n/имя-[ИМЯ] \n/пароль-[ПАРОЛЬ] \n/телефон-[ТЕЛЕФОН] \nНа пример: /логин-PizzaMaster777";
            }
            else if (message.Text.Length > 8 && message.Text.Substring(0, 7) == "/логин-" && User.Login is null)
            {
                string login = message.Text.Substring(7);
                if (login is null || login.Length <= 3)
                {
                    answer = $"Логин не может состоять из {login.Length} символов!\nМинимальная длинна - 3 символа!";
                }
                else
                {
                    var usersTableService = new UsersTableService();
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
                        User.Login = login;
                        answer = "Данные введены верно!";
                    }
                    else
                    {
                        answer = "Пользователь с таким логином уже существует";
                    }

                }

            }
            else if (message.Text.Length > 9 && message.Text.Substring(0, 8) == "/пароль-" && User.Password is null)
            {
                string password = message.Text.Substring(8);
                if (password is null || password.Length <= 3)
                {
                    answer = $"Пароль не может состоять из {password.Length} символов!\nМинимальная длинна - 3 символа!";
                }
                else
                {
                    User.Password = password;
                    answer = "Данные введены верно!";
                }

            }
            else if (message.Text.Length > 6 && message.Text.Substring(0, 5) == "/имя-" && User.FullName is null)
            {
                string name = message.Text.Substring(5);
                if (name is null || name.Length < 2)
                {
                    answer = $"Имя не может состоять из {name.Length} символов!";
                }
                else
                {
                    User.FullName = name;
                    answer = "Данные введены верно!";
                }

            }
            else if (message.Text.Length > 10 && message.Text.Substring(0, 9) == "/телефон-" && User.PhoneNumber is null)
            {
                string phone = message.Text.Substring(9);
                if (phone is null || phone.Length < 12)
                {
                    answer = "Пожалуйста, введите Ваш номер телефона в формате +XYYYXXXYYXX";
                }
                else
                {
                    User.PhoneNumber = phone;
                    answer = "Данные введены верно!";
                }
            }
            else
            {
                answer = "Неверный ввод!";
            }

            await TelegramBot.SendTextMessageAsync(message.Chat.Id, answer);

        }
    }
}
