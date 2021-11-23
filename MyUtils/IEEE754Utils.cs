using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUtils
{
    public static class IEEE754Utils
    {
        public static string FloatToHex(float paraFloat)
        {
            StringBuilder sb = new StringBuilder();
            byte[] bytes = BitConverter.GetBytes(paraFloat);

            foreach (var item in bytes)
            {
                sb.Insert(0, item.ToString("X2"));
            }
            return sb.Insert(0, "0X").ToString();
        }

        /// <summary>
        /// 把4个字节的16进制字符串转化成一个浮点数
        /// </summary>
        /// <param name="hexStr">待转化字符串</param>
        /// <returns>返回浮点数（转化失败返回浮点数最小值）</returns>
        public static float HexToFloat(string hexStr)
        {

            hexStr = hexStr.Trim().ToUpper();
            if (hexStr.StartsWith("0X") || hexStr.StartsWith("0x"))
            {
                hexStr = hexStr.Substring(2);
            }
            if (hexStr.Length != 8)
            {
                return float.MinValue;
            }
            byte[] bytes = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                bytes[i] = Convert.ToByte(hexStr.Substring((3 - i) * 2, 2), 16);
            }
            return BitConverter.ToSingle(bytes, 0);
        }

        public static byte[] FloatToBtyes(float paraFloat)
        {
            return BitConverter.GetBytes(paraFloat);
        }

        public static float ByteToFloat(byte[] bytes)
        {
            if (bytes.Length != 4)
            {
                return float.MinValue;
            }
            return BitConverter.ToSingle(bytes, 0);
        }
    }
}
