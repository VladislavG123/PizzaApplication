using PizzaApp.Services.Abstract;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Args;
using System.Text.RegularExpressions;
using PizzaApp.Models;

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
            User user = new User();
            TelegramBot.OnMessage += BotOnMessageReceived;
            TelegramBot.StartReceiving(new UpdateType[] { UpdateType.Message });
            return user;
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
                User.Login = message.Text.Substring(7);
                answer = User.Login;
            }
            else if (message.Text.Length > 9 && message.Text.Substring(0, 8) == "/пароль-" && User.Password is null)
            {
                User.Password = message.Text.Substring(8);
                answer = User.Password;
            }
            else if (message.Text.Length > 6 && message.Text.Substring(0, 5) == "/имя-" && User.FullName is null)
            {
                User.FullName = message.Text.Substring(5);
                answer = User.FullName;
            }
            else if (message.Text.Length > 10 && message.Text.Substring(0, 9) == "/телефон-" && User.PhoneNumber is null)
            {
                User.PhoneNumber = message.Text.Substring(9);
                answer = User.PhoneNumber;
            }
            
            else
            {
                answer = "Неверный ввод!";
            }

            await TelegramBot.SendTextMessageAsync(message.Chat.Id, answer);

        }
    }
}
