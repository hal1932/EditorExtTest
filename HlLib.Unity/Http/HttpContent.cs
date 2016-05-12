using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HlLib.Unity.Http
{
    public abstract class HttpContent
    {
        public string ContentType { get; protected set; }
        public byte[] Data { get; protected set; }
    }

    public class StringContent : HttpContent
    {
        public StringContent(string data, string contentType = "text/plain")
        {
            ContentType = contentType;
            Data = Encoding.ASCII.GetBytes(data);
        }

        public StringContent(string data, Encoding encoding, string contentType = "text/plain")
        {
            ContentType = contentType;
            Data = encoding.GetBytes(data);
        }
    }

    public class JsonContent : StringContent
    {
        public JsonContent(string data)
            : base(data, Encoding.UTF8, "application/json")
        { }
    }

    public class FormUrlEncodedContent : HttpContent
    {
        public FormUrlEncodedContent(IDictionary<string, string> data)
        {
            ContentType = "application/x-www-form-urlencoded";
            Data = GetBytes(data);
        }

        public FormUrlEncodedContent(object data)
        {
            ContentType = "application/x-www-form-urlencoded";
            var dataType = data.GetType();
            var dataDic = dataType.GetProperties()
                .Where(prop => prop.CanRead)
                .ToDictionary(prop => prop.Name, prop => prop.GetValue(data, null).ToString());
            Data = GetBytes(dataDic);
        }

        private byte[] GetBytes(IDictionary<string, string> data)
        {
            return Encoding.ASCII.GetBytes(
                string.Join("&", data.Select(item => item.Key + "=" + item.Value).ToArray()));
        }
    }

}
