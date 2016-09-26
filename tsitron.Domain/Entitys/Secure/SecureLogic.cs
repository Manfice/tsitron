using System.Security.Cryptography;
using System.Text;

namespace tsitron.Domain.Entitys.Secure
{
    public class SecureLogic
    {
        public static string EncodePassToHash(string pass)
        {
            var md5Provider = new MD5CryptoServiceProvider();
            var passBytes=md5Provider.ComputeHash(Encoding.Default.GetBytes(pass));
            
            var result = new StringBuilder();
            for (var i = 0; i < passBytes.Length; i++)
            {
                result.Append(passBytes[i].ToString("x2"));
            }
            return result.ToString();
        } 
    }

    public class ValidEvents
    {
        public int Code { get; set; }
        public string Message { get; set; }
    }
}