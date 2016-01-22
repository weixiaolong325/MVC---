using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace MVC留言板
{
    //json序列化和反序列化辅助类
    public class jsonHelper
    {
        //json序列化
        public static string JsonSerializer<T>(T t)
        {
             string jsonString="";
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream())
            {
                ser.WriteObject(ms, t);
                jsonString= Encoding.UTF8.GetString(ms.ToArray());
            }
            //替换Json的Date字符串
            string p = @"\\/Date\((\d+)\+\d+\)\\/";
            MatchEvaluator matchEvaluator = new MatchEvaluator(ConvertJsonDateToDateString);
            Regex reg = new Regex(p);
            jsonString = reg.Replace(jsonString, matchEvaluator);
            return jsonString;
        }
        //json反序列化
        public static T JsonDeserialize<T>(string jsonString)
        {
            //将"yyyy-MM-dd HH:mm:ss"格式的字符串转为"\/Date(1294499956278+0800)\/"格式

          string p = @"\d{4}-\d{2}-\d{2}\s\d{2}:\d{2}:\d{2}";
          MatchEvaluator matchEvaluator = new MatchEvaluator(ConvertDateStringToJsonDate);
          Regex reg = new Regex(p);
          jsonString = reg.Replace(jsonString, matchEvaluator);
          DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
          MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
          T obj = (T)ser.ReadObject(ms);
          return obj;
        }
        //将Json序列化的时间由/Date(1294499956278+0800)转为字符串
        private static string ConvertJsonDateToDateString(Match m)
        {
            string result = string.Empty;
            DateTime dt = new DateTime(1970, 1, 1);
            dt = dt.AddMilliseconds(long.Parse(m.Groups[1].Value));
            dt = dt.ToLocalTime();
            result = dt.ToString("yyyy-MM-dd HH:mm:ss");
            return result;
        }
        /// <summary>
        /// 将时间字符串转化为Json时间
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        private static string ConvertDateStringToJsonDate(Match m)
        {
            string result = string.Empty;
            DateTime dt = DateTime.Parse(m.Groups[0].Value);
            dt = dt.ToUniversalTime();
            TimeSpan ts = dt - DateTime.Parse("1970-01-01");
            result = string.Format("\\/Date({0}+0800)\\/", ts.TotalMilliseconds);
            return result;
        }
    }
}