using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace LNBT.Util
{
    internal class PasswordUtil
    {
        private static readonly string secretKey = "2cc856d4f1d146bea4c1897b7d4392f3861354405eaede0409081140b0791fb1";
        private static readonly string iv = "a104752cb5444aded45dc3fe0ddda962";

        public static string EncryptPassword(string password)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = HexToByteArray(secretKey);
                aes.IV = HexToByteArray(iv);

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                        {
                            sw.Write(password);
                        }
                    }
                    byte[] encrypted = ms.ToArray();
                    return Convert.ToBase64String(encrypted);
                }
            }
        }

        public static string DecryptPassword(string encryptedPassword)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = HexToByteArray(secretKey);
                aes.IV = HexToByteArray(iv);

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(encryptedPassword)))
                {
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader sr = new StreamReader(cs))
                        {
                            return sr.ReadToEnd();
                        }
                    }
                }
            }
        }

        private static byte[] HexToByteArray(string hex)
        {
            int length = hex.Length;
            byte[] bytes = new byte[length / 2];
            for (int i = 0; i < length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }
            return bytes;
        }
    }
}
