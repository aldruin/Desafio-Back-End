using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;


namespace BancoApi.Domain.Base;
public class SecurityUtils
{
    public static string HashSHA256(string text)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] byteV = Encoding.UTF8.GetBytes(text);
            byte[] byteH = sha256.ComputeHash(byteV);

            return Convert.ToBase64String(byteH);
        }
    }
}