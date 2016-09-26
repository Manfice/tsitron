using Twilio;

namespace tsitron.App_Data.SMS
{
    public class Logic
    {
        public string Sid { get; } = "AC5411fd69ec0166766d03d62a6807d04f";
        public string AccTocken { get; } = "f074b0389d886b821f437603d94e4877";

        public string SendSms(string from, string to, string message)
        {
            var twilio = new TwilioRestClient(Sid, AccTocken);
            return twilio.SendMessage(from, to, message).Sid;
        }

        public string SendSms(string message)
        {
            var sms = new ASPSMSX2.ASPSMSX2SoapClient("ASPSMSX2Soap");
            var test = sms.CheckCredits("HRQCTEHN6ZHR", "1q2w3eOP");
            var result = sms.SendSimpleTextSMS("HRQCTEHN6ZHR", "1q2w3eOP", "79034441684", "79283731613", message);
            sms.Close();
            return result;
        }
    }
}