using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Security.Cryptography;

namespace ApiSharedMed.Services
{
    public static class HashServices
    {
        public static byte[] GetSaltCode()
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        public static string HashPassword(string password, byte[] saltcode)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
               password: password,
               salt: saltcode,
               prf: KeyDerivationPrf.HMACSHA1,
               iterationCount: 10000,
               numBytesRequested: 256 / 8));
        }
     
    }
}
