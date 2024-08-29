using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.SalesDepartment
{
    public class SalesMessageInfo
    {
        private int infoID;
        private int memID;
        private string memName;
        private int salerID;
        //private string salerName;

        private string subject;
       // private string content;
        private string issuer;
        private short issuedFrom;
        private DateTime issuedDate;
        private bool read;
        private string reader;
        private string readDate;
        private bool isDelete;


        public SalesMessageInfo() { }

        public SalesMessageInfo(int infoID, int memID, string memName, int salerID, string subject, string issuer, short issuedFrom, DateTime issuedDate, bool read)
        {
            this.infoID = infoID;
            this.memID = memID;
            this.memName = memName;
            this.salerID = salerID;
            this.subject = subject;
            this.issuer = issuer;
            this.issuedFrom = issuedFrom;
            this.issuedDate = issuedDate;
            this.read = read;
        }

        public int InfoID
        {
            get { return infoID; }
            set { infoID = value; }
        }
        public int MemID
        {
            get { return memID; }
            set { memID = value; }
        }
        public string MemName
        {
            get { return memName; }
            set { memName = value; }
        }
        public int SalerID
        {
            get { return salerID; }
            set { salerID = value; }
        }

        public string Issuer
        {
            get { return issuer; }
            set { issuer = value; }
        }
        /// <summary>
        /// 主题
        /// </summary>
        public string Subject
        {
            get { return subject; }
            set { subject = value; }
        }
        /// <summary>
        /// 来源0客服留言，１企业留言
        /// </summary>
        public short IssuedFrom
        {
            get { return issuedFrom; }
            set { issuedFrom = value; }
        }
        public string IssuedFromStr
        {
            get
            {
                if (IssuedFrom == 0)
                    return "客服";
                if (IssuedFrom == 1)
                    return "企业";
                return "此前服务代表";
            }
        }
        /// <summary>
        /// 发表日期
        /// </summary>
        public string IssuedDate
        {
            get { return issuedDate.ToString("g"); }
        }
        /// <summary>
        ///　以读
        /// </summary>
        public bool Read
        {
            get { return read; }
            set { read = value; }
        }
        public string ReadStr
        {
            get
            {
                if (Read)
                    return "已读";
                return "未读";
            }
        }
        /// <summary>
        /// 阅读人
        /// </summary>
        public string Reader
        {
            get { return reader; }
            set { reader = value; }
        }
        /// <summary>
        /// 阅读时间
        /// </summary>
        public string ReadDate
        {
            get { return readDate; }
            set { readDate = value; }
        }
        /// <summary>
        /// 被删除
        /// </summary>
        public bool IsDelete
        {
            get { return isDelete; }
            set { isDelete = value; }
        }

    }
}
