using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Goodjob.Common;

namespace Model.SalesDepartment
{
    public class SalesCompanyListInfo
    {
        #region  variables...

        private int memID;
        private string memName;
        private int addressP;
        private int addressC;
        private DateTime registerDate;
        private DateTime updateDate;
        private DateTime lastLoginDate;
        private short memberClass;

        #endregion

        /// <summary>
        /// 公共库企业
        /// </summary>
        public SalesCompanyListInfo(int memID, string memName, int address_P, int address_C, DateTime registerDate, DateTime updateDate, short memberClass, DateTime lastLoginDate)
        {
            this.memID = memID;
            this.memName = memName;
            addressP = address_P;
            addressC = address_C;
            this.registerDate = registerDate;
            this.updateDate = updateDate;
            this.memberClass = memberClass;
            this.lastLoginDate = lastLoginDate;
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


        #endregion
    }
}
