using System.Security.Cryptography;
using System.Text;

namespace Common.Tool
{
    /// <summary>
    /// 加密解密
    /// </summary>
    public static class EnAndDecryption
    {

        public static byte[] Keys = new byte[]
        {
            18,
            52,
            86,
            120,
            144,
            171,
            205,
            239
        };

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Md5Decryption(string str)
        {
            StringBuilder sb = new StringBuilder();
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(str);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="psw"></param>
        /// <returns></returns>
        public static string Decryption(string psw)
        {
            string result=string.Empty;
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes("#%&*)&%@");
                byte[] keys = Keys;
                byte[] array = Convert.FromBase64String(psw);
                DESCryptoServiceProvider descryptoServiceProvider = new DESCryptoServiceProvider();
                MemoryStream memoryStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(memoryStream, descryptoServiceProvider.CreateDecryptor(bytes, keys), CryptoStreamMode.Write);
                cryptoStream.Write(array, 0, array.Length);
                cryptoStream.FlushFinalBlock();
                result = Encoding.UTF8.GetString(memoryStream.ToArray());
                Console.WriteLine(result);
            }
            catch
            {
                result = "j1RRdoP1oR/iMtns+KwKV4vy+mKL36adCh06VZxHD750Tqy6vcQFWj8tVtGJVJgR";
            }
            return result;
        }


        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Encryption(string str)
        {
            string result = string.Empty;
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes("#%&*)&%@".Substring(0, 8));
                byte[] keys = Keys;
                byte[] bytes2 = Encoding.UTF8.GetBytes(str);
                DESCryptoServiceProvider descryptoServiceProvider = new DESCryptoServiceProvider();
                MemoryStream memoryStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(memoryStream, descryptoServiceProvider.CreateEncryptor(bytes, keys), CryptoStreamMode.Write);
                cryptoStream.Write(bytes2, 0, bytes2.Length);
                cryptoStream.FlushFinalBlock();
                result = Convert.ToBase64String(memoryStream.ToArray());
            }
            catch
            {
                result = "";
            }
            return result;
        }
        
    }
}
