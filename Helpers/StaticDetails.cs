using System;
using System.Linq;

namespace Advansio.API.Helpers
{
    public class StaticDetails
    {
        public const decimal DefaultAmount = 0.00M;
        public const string AccountNo = "AccountNumber";
        public static string GenerateAccountNo()
        {
            var random = new Random();
            var chars = "0123456789";
            return new string(chars.Select(c => chars[random.Next(chars.Length)]).Take(10).ToArray());
        }
    }
}