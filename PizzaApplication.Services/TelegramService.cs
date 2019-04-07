using PizzaApp.Services.Abstract;
using Telegram.Bot;
namespace PizzaApp.Services
{
    public class TelegramService : IRegistrationService
    {
        public TelegramBotClient TelegramBot { get; set; }
        readonly string _token;

        public TelegramService()
        {

            _token = "687100641:AAGsutNjTbKPXtIvrKM949hecqyM0o_Q0JM";
            TelegramBot = new TelegramBotClient(_token);
        }
        public bool SendMessage(string phoneNumber)
        {

            return false;
        }
    }
}
