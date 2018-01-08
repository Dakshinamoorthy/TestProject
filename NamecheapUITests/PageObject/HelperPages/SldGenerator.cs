using System;
using System.Security.Cryptography;
using System.Text;
namespace NamecheapUITests.PageObject.HelperPages
{
    public class SldGenerator
    {
        public int GetRandomNumber(int maxNumber)
        {
            if (maxNumber < 1)
                throw new Exception("The maxNumber value should be greater than 1");
            var b = new byte[4];
            new RNGCryptoServiceProvider().GetBytes(b);
            var seed = (b[0] & 0x7f) << 24 | b[1] << 16 | b[2] << 8 | b[3];
            var r = new Random(seed);
            return r.Next(1, maxNumber);
        }
        public string GetRandomString(int length)
        {
            var array = new[]
            {
                "0", "1", "2", "3", "4", "5", "6", "7", "8", "9",
                "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u",
                "v", "w", "x", "y", "z",
                "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U",
                "V", "W", "X", "Y", "Z"
            };
            var sb = new StringBuilder();
            for (var i = 0; i < length; i++)
                sb.Append(array[GetRandomNumber(62)]);
            return sb.ToString().ToLowerInvariant();
        }
        public string GetRandomAlphabets(int length)
        {
            var array = new[]
            {
                "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p"
                , "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "A", "B", "C", "D", "E",
                "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"
            };
            var sb = new StringBuilder();
            for (var i = 0; i < length; i++)
                sb.Append(array[GetRandomNumber(52)]);
            return sb.ToString();
        }
        public StringBuilder GetRandomDigits(int length)
        {
            var randomNum = new StringBuilder(length);
            for (var startTelcount = 0; startTelcount <= length; startTelcount++)
            {
                var r = new Random();
                var telCount = r.Next(0, 9);
                randomNum = randomNum.Append(telCount.ToString());
            }
            return randomNum;
        }
    }
}