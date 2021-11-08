using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace streamer.db.Database.Helpers
{
    public static class Crypto
    {

        private const string Vector = "etacomsomevector";
        private const string Key = "0123456789abcdef";

        public static string EncryptString(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return "";
            }

            try
            {

                var key = Encoding.UTF8.GetBytes(Key);
                byte[] iv = Encoding.ASCII.GetBytes(Vector);

                using (var aesAlg = Aes.Create())
                {
                    using (var encryptor = aesAlg.CreateEncryptor(key, iv))
                    {
                        using (var msEncrypt = new MemoryStream())
                        {
                            using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                            using (var swEncrypt = new StreamWriter(csEncrypt))
                            {
                                swEncrypt.Write(text);
                            }

                            var decryptedContent = msEncrypt.ToArray();

                            var result = new byte[iv.Length + decryptedContent.Length];

                            Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                            Buffer.BlockCopy(decryptedContent, 0, result, iv.Length, decryptedContent.Length);

                            return Convert.ToBase64String(result);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Unable to encrypt string: " + text, e);
                return "";
            }
        }

        public static string DecryptString(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText))
            {
                return "";
            }

            try
            {
                var fullCipher = Convert.FromBase64String(cipherText);

                byte[] iv = Encoding.ASCII.GetBytes(Vector);
                var cipher = new byte[fullCipher.Length - iv.Length];

                Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
                Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, fullCipher.Length - iv.Length);
                var key = Encoding.UTF8.GetBytes(Key);

                using (var aesAlg = Aes.Create())
                {
                    using (var decryptor = aesAlg.CreateDecryptor(key, iv))
                    {
                        string result;
                        using (var msDecrypt = new MemoryStream(cipher))
                        {
                            using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                            {
                                using (var srDecrypt = new StreamReader(csDecrypt))
                                {
                                    result = srDecrypt.ReadToEnd();
                                }
                            }
                        }

                        return result;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Unable to decrypt string: " + cipherText, e);
                return "";
            }
        }
    }
}
