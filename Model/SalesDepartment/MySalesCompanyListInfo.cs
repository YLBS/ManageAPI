using Goodjob.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.SalesDepartment
{
    /// <summary>
    /// 我的企业
    /// </summary>
    public class MySalesCompanyListInfo
    {
        #region  variables...

        private int memID;
        private string memName;
        private int addressP;
        private int addressC;
        private int maxPosNum;
        private int maxResumeNum;
        private string salerName;
        private DateTime endValidDate;
        private DateTime beginValidDate;
        private DateTime registerDate;
        private DateTime updateDate;
        private DateTime lastLoginDate;
        private short memberClass;
        private int receivedResumeCount;
        private string contanctTel;
        private bool mapped;

        #endregion
        /// <summary>
        ///  我的企业列表。 业务员企业or部门企业(包括收到简历数)
        /// </summary>
        public MySalesCompanyListInfo(int memID, string memName, int address_P, int address_C, int maxPosNum, int maxResumeNum, DateTime beginValidDate, DateTime endValidDate, short memberClass, DateTime lastLoginDate, string SalerUserName, int ResumeCount, string contactTel, bool HasMapped)
        {
            this.memID = memID;
            this.memName = memName;
            this.addressP = address_P;
            this.addressC = address_C;
            this.maxPosNum = maxPosNum;
            this.maxResumeNum = maxResumeNum;
            this.beginValidDate = beginValidDate;
            this.endValidDate = endValidDate;
            this.memberClass = memberClass;
            this.lastLoginDate = lastLoginDate;
            this.salerName = SalerUserName;
            this.receivedResumeCount = ResumeCount;
            this.contanctTel = contactTel;
            this.mapped = HasMapped;
        }
        #region attributes.....

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
        public string MemberClassName
        {
            get
            {
                return NameOtherProvider.GetMemberClassName(MemberClass);
            }
        }
        public int AddressP
        {
            get { return addressP; }
            set { addressP = value; }
        }
        public int AddressC
        {
            get { return addressC; }
            set { addressC = value; }
        }

        public string AddressStrName
        {
            get
            {
                return NameProvider.GetProvinceName(AddressP) + " " + NameProvider.GetCityName(AddressC);
            }
        }
        public DateTime RegisterDate
        {
            get { return registerDate; }
            set { registerDate = value; }
        }
        public DateTime UpdateDate
        {
            get { return updateDate; }
            set { updateDate = value; }
        }
        public string GetRegisterAndUpdate
        {
            get
            {
                return RegisterDate.ToString("d") + "/" + UpdateDate.ToString("d");
            }
        }
        public DateTime LastLoginDate
        {
            get { return lastLoginDate; }
            set { lastLoginDate = value; }
        }
        public string LastLoginDateStr
        {
            get
            {
                return LastLoginDate.ToString("g");
            }
        }
        public short MemberClass
        {
            get { return memberClass; }
            set { memberClass = value; }
        }
        public int MaxPosNum
        {
            get { return maxPosNum; }
            set { maxPosNum = value; }
        }

        public DateTime BeginValidDate
        {
            get { return beginValidDate; }
            set { beginValidDate = value; }
        }
        public DateTime EndValidDate
        {
            get { return endValidDate; }
            set { endValidDate = value; }
        }
        public string ValidDateStr
        {
            get { return BeginValidDate.ToString("d") + "/" + EndValidDate.ToString("d"); }
        }

        public string SalerName
        {
            get { return salerName; }
            set { salerName = value; }
        }

        public int ReceivedResumeCount
        {
            set { this.receivedResumeCount = value; }
            get { return this.receivedResumeCount; }
        }

        public bool Mapped
        {
            set { this.mapped = value; }
            get { return this.mapped; }
        }
        public string ContanctTel
        {
            get { return contanctTel; }
            set { contanctTel = value; }
        }
        #endregion
    }
}
