using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PizzaApp.Services.Abstract;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace PizzaApp.Services
{
    public class TwillioService : IRegistrationService
    {
        private int _secretCode;


        public bool SendMessage(string phoneNumber)
        {
            try
            {
                _secretCode = Convert.ToInt32(new Random().Next(1000, 9999));
                const string ACCOUNT_SID = "ACa902f79f4063bfa4e4da8b2930f931b2";
                const string AUTH_TOKEN = "55ce5d687fbdf981f550141835655fbf";

                TwilioClient.Init(ACCOUNT_SID, AUTH_TOKEN);

                var message = MessageResource.Create(
                    body: $"Code: {_secretCode}",
                    from: new Twilio.Types.PhoneNumber("+13022447724"),
                    to: new Twilio.Types.PhoneNumber(phoneNumber)
                );
            }
            catch (Exception)
            {
                return false;
            }
            return true;

        }


        public bool IsRightCode(int code)
        {
            return _secretCode == code ? true : false;
        }

    }
}
