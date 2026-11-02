using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace VirtualBenchPro_v4.Classes
{
    public class HexFileCrypto
    {
        public static string password = "GuessWhatisthekey123!";
        // 加密HEX文件
        public static void EncryptHexFile(string inputHexPath, string outputEncryptedPath, string password)
        {
            // 读取原始HEX文件内容
            string hexContent = File.ReadAllText(inputHexPath);
            // 生成随机盐值
            byte[] salt = GenerateRandomSalt();
            // 从密码和盐值生成密钥和IV
            Rfc2898DeriveBytes keyDerivation = null;
            byte[] key = null;
            byte[] iv = null;
            try
            {
                keyDerivation = new Rfc2898DeriveBytes(password, salt, 10000);
                key = keyDerivation.GetBytes(32); // 256位密钥
                iv = keyDerivation.GetBytes(16);  // 128位IV
                                                  // 加密数据
                byte[] encryptedData;
                using (Aes aes = Aes.Create())
                {
                    aes.Key = key;
                    aes.IV = iv;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        // 写入盐值(未加密)
                        ms.Write(salt, 0, salt.Length);
                        using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                        using (StreamWriter sw = new StreamWriter(cs))
                        {
                            sw.Write(hexContent);
                        }
                        encryptedData = ms.ToArray();
                    }
                }
                // 写入加密文件
                File.WriteAllBytes(outputEncryptedPath, encryptedData);
            }
            finally
            {
                // 清理敏感数据
                if (key != null) Array.Clear(key, 0, key.Length);
                if (iv != null) Array.Clear(iv, 0, iv.Length);
                keyDerivation?.Dispose();
            }
        }
        // 解密HEX文件
        public static bool DecryptHexFile(string inputEncryptedPath, string outputHexPath, string password)
        {
            bool success = false;

            // 读取加密文件
            byte[] encryptedData = File.ReadAllBytes(inputEncryptedPath);
            // 提取盐值(前16字节)
            byte[] salt = new byte[16];
            Array.Copy(encryptedData, 0, salt, 0, salt.Length);
            // 从密码和盐值生成密钥和IV
            Rfc2898DeriveBytes keyDerivation = null;
            byte[] key = null;
            byte[] iv = null;

            try
            {
                keyDerivation = new Rfc2898DeriveBytes(password, salt, 10000);
                key = keyDerivation.GetBytes(32); // 256位密钥
                iv = keyDerivation.GetBytes(16);  // 128位IV
                                                    // 解密数据
                using (Aes aes = Aes.Create())
                {
                    aes.Key = key;
                    aes.IV = iv;
                    using (MemoryStream ms = new MemoryStream(encryptedData, salt.Length, encryptedData.Length - salt.Length))
                    using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read))
                    using (StreamReader sr = new StreamReader(cs))
                    using (StreamWriter sw = new StreamWriter(outputHexPath))
                    {
                        string hexContent = sr.ReadToEnd();
                        sw.Write(hexContent);
                    }
                }
                success = true;
            }
            catch (Exception ex)
            {
                success = false;
            }
            finally
            {
                // 清理敏感数据
                if (key != null) Array.Clear(key, 0, key.Length);
                if (iv != null) Array.Clear(iv, 0, iv.Length);
                keyDerivation?.Dispose();
            }
            return success;
        }

        // 生成随机盐值
        private static byte[] GenerateRandomSalt()
        {
            byte[] salt = new byte[16];
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }
    }
}
