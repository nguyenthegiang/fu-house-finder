using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HouseFinder_API.Helper
{
    public class Hashing
    {
        public static string Encrypt(string rawPassword)
        {
            if (rawPassword == null) return null;
            using var crypt = SHA256.Create();
            byte[] bytes = crypt.ComputeHash(Encoding.UTF8.GetBytes(rawPassword));
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }
}
