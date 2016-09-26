using Twilio;
namespace tsitron.Domain.SMS
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
    }
}