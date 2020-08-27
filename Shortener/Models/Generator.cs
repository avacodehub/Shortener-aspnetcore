using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shortener.Models
{
    public static class Generator
    {
        static readonly int LENGHT = 6;
        static readonly int LENGHTLONG = 7;
        
        public static string GetString()
        {
            return GenerateString(LENGHT);
        }

        public static string GetStringLong()
        {
            return GenerateString(LENGHTLONG);
        }


        public static string GenerateString(int len)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[len];
            var random = new Random();

            for (int i = 0; i < len; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            return new string(stringChars);
        }
    }
}
