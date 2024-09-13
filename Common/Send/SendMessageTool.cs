using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace Commons.Send
{
    public static class SendMessageTool
    {
        public static string SendMessage(string userid, string password, string desbumbers, string msg, string sendTime)
        {
            string qstr = "userid=" + HttpUtility.UrlEncode(userid, Encoding.UTF8);
            qstr += "&password=" + HttpUtility.UrlEncode(password, Encoding.UTF8);
            qstr += "&destnumbers=" + HttpUtility.UrlEncode(desbumbers, Encoding.UTF8);
            qstr += "&msg=" + HttpUtility.UrlEncode(msg, Encoding.UTF8);
            qstr += "&sendtime=" + HttpUtility.UrlEncode(sendTime, Encoding.UTF8);
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("http://server4.chineseserver.net:9801/CASServer/SmsAPI/SendMessage.jsp?" + qstr);
            req.Method = "GET";
            using (HttpWebResponse wr = (HttpWebResponse)req.GetResponse())
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(wr.GetResponseStream());
                return xmlDoc.DocumentElement.OuterXml;
            }
        }
    }
}
