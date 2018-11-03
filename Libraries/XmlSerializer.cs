using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Libraries
{
    public static class XmlSerializer
    {
        ///// <summary>
        ///// 직렬화된 xml파일에서 객체를 읽어옵니다.
        ///// </summary>
        ///// <typeparam name="T">읽어올 객체의 타입입니다.</typeparam>
        ///// <param name="fileName">xml파일의 경로입니다.</param>
        ///// <returns>만들어진 객체를 반환합니다.</returns>
        public static T Deserial<T>(string xmlStr) where T : new()
        {
            var stringReader = new System.IO.StringReader(xmlStr);
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
            return (T)serializer.Deserialize(stringReader);
        }
        
        /// <summary>
        /// 객체를 직렬화해 xml 문자열로 반환합니다.
        /// </summary>
        /// <typeparam name="T">객체의 타입입니다.</typeparam>
        /// <param name="result">직렬화 된 xml문자열입니다.</param>
        /// <returns></returns>
        public static string Serialize<T>(T result)
        {
            var stringwriter = new System.IO.StringWriter();
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
            serializer.Serialize(stringwriter, result);
            return stringwriter.ToString();
        }
    }
}
