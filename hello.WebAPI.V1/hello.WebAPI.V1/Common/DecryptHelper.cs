using System;
using System.Security.Cryptography;
using System.Text;

namespace hello.WebAPI.V1.Common
{
    // 解密相关
    public class DecryptHelper
    {
        // 字符编码格式:UTF-8
        private static Encoding _encoding = Encoding.UTF8;
        /// <summary>
        /// Base64解密，采用utf8编码方式解密
        /// </summary>
        /// <param name="result">待解密的密文</param>
        /// <returns>解密后的字符串</returns>
        public static string Base64Decode(string result)
        {
            return Base64Decode(_encoding, result);
        }

        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="encodeType">解密采用的编码方式，注意和加密时采用的方式一致</param>
        /// <param name="result">待解密的密文</param>
        /// <returns>解密后的字符串</returns>
        public static string Base64Decode(Encoding encodeType, string result)
        {
            string decode = string.Empty;
            byte[] bytes = Convert.FromBase64String(result);
            try
            {
                decode = encodeType.GetString(bytes);
            }
            catch
            {
                decode = result;
            }
            return decode;
        }

        /// <summary>
        /// RSA解密
        /// </summary>
        /// <param name="privatekey"></param>
        /// <param name="mw"></param>
        /// <returns></returns>
        public static string RSADecrypt(string privatekey, string mw)
        {
            RSACryptoServiceProvider ras = new RSACryptoServiceProvider();
            ras.FromXmlString(privatekey);
            byte[] bytes = ras.Decrypt(Convert.FromBase64String(mw), false);
            return _encoding.GetString(bytes);
        }
    }
}