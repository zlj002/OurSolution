using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace BizProcess.Common
{
    public class DESECBPKCS5Encrypt
    {
        public string Encrypt(string str, string key)
        {
            if (key.Length != 8)
            {
                throw new Exception("key长度必须为8");
            }
            try
            {
                StringBuilder sb = new StringBuilder();
                byte[] data = Encoding.UTF8.GetBytes(str);
                byte[] kbs = Encoding.UTF8.GetBytes(key);
                int n = data.Length / 8 + 1;
                for (int i = 0; i < n; i++)
                {
                    int startIndex = i * 8;
                    int endIndex = (i + 1) * 8;
                    if (endIndex >= data.Length)
                    {
                        endIndex = data.Length;
                    }
                    List<byte> bs = new List<byte>();
                    for (int j = startIndex; j < endIndex; j++)
                    {
                        bs.Add(data[j]);
                    }
                    string result = Encrypt(bs.ToArray(), kbs);
                    result = result.Length > 16 ? result.Substring(0, 16) : result;
                    sb.Append(result);
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("DES加密异常" + ex);
            }
        }

        private string Encrypt(byte[] data, byte[] key)
        {
            MemoryStream mStream = new MemoryStream();
            DESCryptoServiceProvider dsp = new DESCryptoServiceProvider();
            dsp.Mode = CipherMode.ECB;
            dsp.Padding = PaddingMode.PKCS7;
            CryptoStream cStream = new CryptoStream(mStream, dsp.CreateEncryptor(key, null), CryptoStreamMode.Write);
            cStream.Write(data, 0, data.Length);
            cStream.FlushFinalBlock();
            byte[] ret = mStream.ToArray();
            cStream.Close();
            mStream.Close();
            return ConvertBsToHex(ret);
        }

        private string ConvertBsToHex(byte[] bs)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in bs)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }
    }
}
