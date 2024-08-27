using Goodjob.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.SalesDepartment
{
    public class CompanyServiceInfo
    {
        #region Member Variables
        private int memID;
        private short memberClass;
        private DateTime beginValidDate;
        private DateTime endValidDate;
        private int maxPosNum;
        private int maxResumeNum;
        private int salerUserID;
        private string salerUserName;
        private string salerTel;
        private string salerEmail;
        private short checkFlag;
        #endregion

        public CompanyServiceInfo()
        {

        }

        #region Constructors
        public CompanyServiceInfo(int memID, short memberClass, DateTime beginValidDate, DateTime endValidDate, int maxPosNum, int maxResumeNum, int salerUserID, string salerUserName, string salerTel, string salerEmail, short checkFlag,string weiXinHao)// 20240827添加微信号
        {
            this.memID = memID;
            this.memberClass = memberClass;
            this.beginValidDate = beginValidDate;
            this.endValidDate = endValidDate;
            this.maxPosNum = maxPosNum;
            this.maxResumeNum = maxResumeNum;
            this.salerUserID = salerUserID;
            this.salerUserName = salerUserName;
            this.salerTel = salerTel;
            this.salerEmail = salerEmail;
            this.checkFlag = checkFlag;
        }
        #endregion

        #region Public Properties

        public int MemID
        {
            get { return memID; }
        }

        public string MemberClassName
        {
            get { return NameOtherProvider.GetMemberClassName(memberClass); }
        }

        public string BeginValidDate
        {
            get { return beginValidDate.ToString("g"); }
        }

        public string EndValidDate
        {
            get { return endValidDate.ToString("g"); }
        }


        public DateTime EVDate
        {
            get { return endValidDate; }
        }

        public int MaxPosNum
        {
            get { return maxPosNum; }
        }

        public int MaxResumeNum
        {
            get { return maxResumeNum; }
        }

        public int SalerUserID
        {
            get { return salerUserID; }
        }

        public string SalerUserName
        {
            get { return salerUserName; }
        }

        public string SalerTel
        {
            get { return salerTel; }
        }

        public string SalerEmail
        {
            get { return salerEmail; }
        }

        public string CheckFlagName
        {
            get { return NameOtherProvider.GetMemberCheckFlagName(checkFlag); }
        }

        #endregion
    }
}
