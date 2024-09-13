using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Commons.Helper;

namespace Commons.Send
{
    public class MailInfo
    {
        private int id;
        private string username;
        private string password;
        private string subject;
        private string body;
        private string server;

        public MailInfo() { }

        public MailInfo(XmlNode node)
        {
            this.id = Int32.Parse(node.Attributes["id"].Value);
            this.username = node.SelectSingleNode("username").InnerText;
            this.password = node.SelectSingleNode("password").InnerText;
            this.subject = node.SelectSingleNode("subject").InnerText;
            this.server = node.SelectSingleNode("server").InnerText;

            string tmpPath = AppSet.TempletPath + node.SelectSingleNode("templet").InnerText;
            this.body = FileHelper.GetFileText(tmpPath);
        }

        public int ID
        {
            get { return id; }
        }

        public string UserName
        {
            get { return username; }
        }

        public string Password
        {
            get { return password; }
        }

        public string Subject
        {
            get { return subject; }
            set { subject = value; }
        }

        public string Body
        {
            get { return body; }
            set { body = value; }
        }

        public string Server
        {
            get { return server; }
        }
    }
}
