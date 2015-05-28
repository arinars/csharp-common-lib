using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace arinars.common
{
    public class AesEncryptUtil
    {
        /// <summary>
        /// AES(Advanced Encryption Standard) 암호화 
        ///  - CBC 모드를 사용한다. (1개앞의 암호문 블록과 평문 블록을 머지후 암호화 수행) 
        /// 
        /// </summary>
        /// <param name="bytesToBeEncrypted"></param>
        /// <param name="passwordBytes"></param>
        /// <returns></returns>
        public static byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            byte[] encryptedBytes = null;

            // Set your salt here, change it to meet your flavor:
            byte[] saltBytes = passwordBytes;
            // Example:
            //saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            // 메모리스트림 객체를 선언,초기화 
            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;          // 키 사이즈 256 Bit (32 Byte)
                    AES.BlockSize = 128;        // 블록/벡터 사이즈 128 Bit (16 Byte)

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000); // (패스워드+ 솔트 + 반복횟수)키 생성 
                    AES.Key = key.GetBytes(AES.KeySize / 8);    // 키 입력
                    AES.IV = key.GetBytes(AES.BlockSize / 8);   // 초기화 벡터   

                    AES.Mode = CipherMode.CBC; // 블록 암호화 모드 

                    // CryptoStream객체를 암호화된 데이터를 쓰기 위한 용도로 선언
                    using (CryptoStream cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length); // 암호화 프로세스 진행 
                        cs.Close(); // 암호화 종료 
                    }
                    encryptedBytes = ms.ToArray(); // 암호화된 데이터를 바이트 배열로 담는다. 
                }
            }

            return encryptedBytes;
        }

        /// <summary>
        /// AES 복호화 
        /// </summary>
        /// <param name="bytesToBeDecrypted"></param>
        /// <param name="passwordBytes"></param>
        /// <returns></returns>
        public static byte[] AES_Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
        {
            byte[] decryptedBytes = null;
            // Set your salt here to meet your flavor:
            byte[] saltBytes = passwordBytes;
            // Example:
            //saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (CryptoStream cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                        cs.Close();
                    }
                    decryptedBytes = ms.ToArray();
                }
            }

            return decryptedBytes;
        }


        /// <summary>
        /// 암호화 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public static string Encrypt(string text, string pwd)
        {
            byte[] originalBytes = Encoding.UTF8.GetBytes(text);
            byte[] encryptedBytes = null;
            byte[] passwordBytes = Encoding.UTF8.GetBytes(pwd);

            // Hash the password with SHA256
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            // Generating salt bytes
            byte[] saltBytes = GetRandomBytes();

            // Appending salt bytes to original bytes
            byte[] bytesToBeEncrypted = new byte[saltBytes.Length + originalBytes.Length];
            for (int i = 0; i < saltBytes.Length; i++)
            {
                bytesToBeEncrypted[i] = saltBytes[i];
            }
            for (int i = 0; i < originalBytes.Length; i++)
            {
                bytesToBeEncrypted[i + saltBytes.Length] = originalBytes[i];
            }

            encryptedBytes = AES_Encrypt(bytesToBeEncrypted, passwordBytes);

            return Convert.ToBase64String(encryptedBytes);
        }

        public static string Decrypt(string decryptedText, string pwd)
        {
            byte[] bytesToBeDecrypted = Convert.FromBase64String(decryptedText);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(pwd);

            // Hash the password with SHA256
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            byte[] decryptedBytes = AES_Decrypt(bytesToBeDecrypted, passwordBytes);

            // Getting the size of salt
            int _saltSize = 4;

            // Removing salt bytes, retrieving original bytes
            byte[] originalBytes = new byte[decryptedBytes.Length - _saltSize];
            for (int i = _saltSize; i < decryptedBytes.Length; i++)
            {
                originalBytes[i - _saltSize] = decryptedBytes[i];
            }

            return Encoding.UTF8.GetString(originalBytes);
        }


        public static byte[] GetRandomBytes()
        {
            int _saltSize = 4;
            byte[] ba = new byte[_saltSize];
            RNGCryptoServiceProvider.Create().GetBytes(ba);
            return ba;
        }


        /// <summary>
        /// 암호화 랩퍼 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="passwordBytes"></param>
        /// <returns></returns>
        public static string Encrypt(string text, byte[] passwordBytes)
        {
            byte[] originalBytes = Encoding.UTF8.GetBytes(text);
            byte[] encryptedBytes = null;

            // Hash the password with SHA256
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            // Getting the salt size
            int saltSize = GetSaltSize(passwordBytes);
            // Generating salt bytes
            byte[] saltBytes = GetRandomBytes(saltSize);

            // Appending salt bytes to original bytes
            byte[] bytesToBeEncrypted = new byte[saltBytes.Length + originalBytes.Length];
            for (int i = 0; i < saltBytes.Length; i++)
            {
                bytesToBeEncrypted[i] = saltBytes[i];
            }
            for (int i = 0; i < originalBytes.Length; i++)
            {
                bytesToBeEncrypted[i + saltBytes.Length] = originalBytes[i];
            }

            encryptedBytes = AES_Encrypt(bytesToBeEncrypted, passwordBytes);

            return Convert.ToBase64String(encryptedBytes);
        }

        /// <summary>
        /// 복호화 랩퍼 
        /// </summary>
        /// <param name="decryptedText"></param>
        /// <param name="passwordBytes"></param>
        /// <returns></returns>
        public static string Decrypt(string decryptedText, byte[] passwordBytes)
        {
            byte[] bytesToBeDecrypted = Convert.FromBase64String(decryptedText);

            // Hash the password with SHA256
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            byte[] decryptedBytes = AES_Decrypt(bytesToBeDecrypted, passwordBytes);

            // Getting the size of salt
            int saltSize = GetSaltSize(passwordBytes);

            // Removing salt bytes, retrieving original bytes
            byte[] originalBytes = new byte[decryptedBytes.Length - saltSize];
            for (int i = saltSize; i < decryptedBytes.Length; i++)
            {
                originalBytes[i - saltSize] = decryptedBytes[i];
            }

            return Encoding.UTF8.GetString(originalBytes);
        }

        /// <summary>
        /// 솔트 사이즈 가져오기 
        /// </summary>
        /// <param name="passwordBytes"></param>
        /// <returns></returns>
        public static int GetSaltSize(byte[] passwordBytes)
        {
            var key = new Rfc2898DeriveBytes(passwordBytes, passwordBytes, 1000);
            byte[] ba = key.GetBytes(2);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < ba.Length; i++)
            {
                sb.Append(Convert.ToInt32(ba[i]).ToString());
            }
            int saltSize = 0;
            string s = sb.ToString();
            foreach (char c in s)
            {
                int intc = Convert.ToInt32(c.ToString());
                saltSize = saltSize + intc;
            }

            return saltSize;
        }

        /// <summary>
        /// 랜덤 바이트 가져오기 
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static byte[] GetRandomBytes(int length)
        {
            byte[] ba = new byte[length];
            RNGCryptoServiceProvider.Create().GetBytes(ba);
            return ba;
        }

        /// <summary>
        /// 텍스트 암호화 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string EncryptText(string input, string password)
        {
            // Get the bytes of the string
            byte[] bytesToBeEncrypted = Encoding.UTF8.GetBytes(input);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            // Hash the password with SHA256
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            byte[] bytesEncrypted = AES_Encrypt(bytesToBeEncrypted, passwordBytes);

            string result = Convert.ToBase64String(bytesEncrypted);

            return result;
        }

        /// <summary>
        /// 텍스트 복호화 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string DecryptText(string input, string password)
        {
            // Get the bytes of the string
            byte[] bytesToBeDecrypted = Convert.FromBase64String(input);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            byte[] bytesDecrypted = AES_Decrypt(bytesToBeDecrypted, passwordBytes);

            string result = Encoding.UTF8.GetString(bytesDecrypted);

            return result;
        }
    }
}
