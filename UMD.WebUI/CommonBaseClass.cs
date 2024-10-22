using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

public class CommonBaseClass : Controller
{
    private readonly string _pepper;

    public CommonBaseClass(IConfiguration configuration)
    {
        _pepper = configuration["Pepper"];
    }

    public string ComputePasswordHash(string userName, string password)
    {
        int keySize = 64;
        int iterations = 1000;
        var hashAlgorithmName = HashAlgorithmName.SHA512; 

        var salt = ComputeSalt(userName);
        using (var rfc2898 = new Rfc2898DeriveBytes(password, salt, iterations, hashAlgorithmName))
        {
            string result = Convert.ToBase64String(rfc2898.GetBytes(keySize));
            return result;
        }
    }

    private byte[] ComputeSalt(string userName)
    {
        byte[] result = Encoding.UTF8.GetBytes(userName + _pepper);
        return result;
    }
}
