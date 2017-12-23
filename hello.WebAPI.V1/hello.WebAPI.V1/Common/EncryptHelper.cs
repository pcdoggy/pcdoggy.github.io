using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace hello.WebAPI.V1.Common
{
    public class EncryptHelper
    {
        // 字符编码格式:UTF-8
        private static Encoding _encoding = Encoding.UTF8;
        private static PaddingMode _paddingMode = PaddingMode.PKCS7;

        /// <summary>
        /// Base64加密，采用utf8编码方式加密
        /// </summary>
        /// <param name="source">待加密的明文</param>
        /// <returns>加密后的字符串</returns>
        public static string Base64Encode(string source)
        {
            return Base64Encode(_encoding, source);
        }

        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="encodeType">加密采用的编码方式</param>
        /// <param name="source">待加密的明文</param>
        /// <returns></returns>
        public static string Base64Encode(Encoding encodeType, string source)
        {
            string encode = string.Empty;
            byte[] bytes = encodeType.GetBytes(source);
            try
            {
                encode = Convert.ToBase64String(bytes);
            }
            catch
            {
                encode = source;
            }
            return encode;
        }

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="source">待加密文本</param>
        /// <returns></returns>
        public static string MD5Encrypt(string source)
        {
            byte[] result = _encoding.GetBytes(source);    // 待加密文本 
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] output = md5.ComputeHash(result);
            return Convert.ToBase64String(output);  // 输出加密文本
        }
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="source">待加密文本</param>
        /// <returns></returns>
        public static string MD5_2Encrypt(string source)
        {
            byte[] result = _encoding.GetBytes(source);    // 待加密文本 
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] output = md5.ComputeHash(result);
            return BitConverter.ToString(output).Replace("-", "").ToUpper();  // 输出加密文本
        }

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="key">加密字符</param>
        /// <param name="toEncrypt">加密的密码</param>
        /// <param name="iv">密钥</param>
        /// <returns></returns>
        public static string AESEncrypt(string key, string toEncrypt, string iv, string aesmode)
        {
            RijndaelManaged rm = new RijndaelManaged();
            rm.Key = Convert.FromBase64String(key);//加密秘钥
            rm.Mode = (CipherMode)Enum.Parse(typeof(CipherMode), aesmode); // CipherMode.CBC;//运算模式
            rm.Padding = _paddingMode;// PaddingMode.PKCS7;//填充方式
            rm.IV = Convert.FromBase64String(iv);//初始化向量,CBC模式是需要,ECB模式不需要
            ICryptoTransform ctran = rm.CreateEncryptor();
            byte[] bytes = _encoding.GetBytes(toEncrypt);
            byte[] enbytes = ctran.TransformFinalBlock(bytes, 0, bytes.Length);
            return Convert.ToBase64String(enbytes);
        }
        /// AES加密
        /// </summary>
        /// <param name="inputdata">输入的数据</param>
        /// <param name="iv">向量128位</param>
        /// <param name="strKey">加密密钥</param>
        /// <returns></returns>
        public static byte[] AESEncrypt(byte[] inputdata, byte[] iv, string strKey)
        {
            //分组加密算法   
            SymmetricAlgorithm des = Rijndael.Create();
            byte[] inputByteArray = inputdata;//得到需要加密的字节数组       
            //设置密钥及密钥向量
            des.Key = _encoding.GetBytes(strKey.Substring(0, 32));
            des.IV = iv;
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    byte[] cipherBytes = ms.ToArray();//得到加密后的字节数组   
                    cs.Close();
                    ms.Close();
                    return cipherBytes;
                }
            }
        }


        #region RSA加密

        /// <summary>  
        /// RSA产生密钥  
        /// </summary>  
        /// <param name="xmlKeys">私钥</param>  
        /// <param name="xmlPublicKey">公钥</param>  
        public static void RSAKey(out string xmlKeys, out string xmlPublicKey)
        {
            try
            {
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                xmlKeys = rsa.ToXmlString(true);
                xmlPublicKey = rsa.ToXmlString(false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //##############################################################################   
        //RSA 方式加密   
        //KEY必须是XML的形式,返回的是字符串   
        //该加密方式有长度限制的！  
        //##############################################################################   

        /// <summary>  
        /// RSA的加密函数  
        /// </summary>  
        /// <param name="xmlPublicKey">公钥</param>  
        /// <param name="encryptString">待加密的字符串</param>  
        /// <returns></returns>  
        public static string RSAEncrypt(string xmlPublicKey, string encryptString)
        {
            try
            {
                byte[] PlainTextBArray;
                byte[] CypherTextBArray;
                string Result;
                System.Security.Cryptography.RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(xmlPublicKey);
                PlainTextBArray = (new UnicodeEncoding()).GetBytes(encryptString);
                CypherTextBArray = rsa.Encrypt(PlainTextBArray, false);
                Result = Convert.ToBase64String(CypherTextBArray);
                return Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>  
        /// RSA的加密函数   
        /// </summary>  
        /// <param name="xmlPublicKey">公钥</param>  
        /// <param name="EncryptString">待加密的字节数组</param>  
        /// <returns></returns>  
        public static string RSAEncrypt(string xmlPublicKey, byte[] EncryptString)
        {
            try
            {
                byte[] CypherTextBArray;
                string Result;
                System.Security.Cryptography.RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(xmlPublicKey);
                CypherTextBArray = rsa.Encrypt(EncryptString, false);
                Result = Convert.ToBase64String(CypherTextBArray);
                return Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}