using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
<<<<<<< HEAD
=======

>>>>>>> parent of c8d1d88 (Merge branch 'main' into final-fixes)
public class DataEncrypter
{
    private static string key = "N7OnL3lf8YasErkERkQAE7+u5R6fspD6QkQZhWhCv/4=";
    private static string iv = "dt9espR+qOm3M5jlfo5uqQ==";
    public static async Task<byte[]> Encrypt(string original)
    {
        byte[] encrypted;
        using (Aes aes = Aes.Create())
        {
            aes.Key = Convert.FromBase64String(key);
            aes.IV = Convert.FromBase64String(iv);

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                    {
                        await streamWriter.WriteAsync(original);
                    }
                    encrypted = memoryStream.ToArray();
                }
            }
        }
        return encrypted;
    }

    public static string Decrypt(byte[] encrypted)
    {
        string decrypted;

        using (Aes aes = Aes.Create())
        {
            aes.Key = Convert.FromBase64String(key);
            aes.IV = Convert.FromBase64String(iv);

            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using (MemoryStream memoryStream = new MemoryStream(encrypted))
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader streamReader = new StreamReader(cryptoStream))
                    {
                        decrypted = streamReader.ReadToEnd();
                    }
                }
            }
        }

        return decrypted;
    }
}
