using Goodjob.Common;

namespace Model.SalesDepartment
{
    /// <summary>
    /// 业务员企业or部门企业
    /// </summary>
    public class SalesCompanyListInfo1
    {
        private int memID;
        private string memName;
        private int addressP;
        private int addressC;
        private int maxPosNum;
        private int maxResumeNum;
        private DateTime beginValidDate;
        private DateTime endValidDate;
        private DateTime lastLoginDate;
        private short memberClass;
        private string salerName;
        private string dptName;
        public SalesCompanyListInfo1(int memID, string memName, int address_P, int address_C, int MaxPosNum, int maxResumeNum, DateTime beginValidDate, DateTime endValidDate, short memberClass, DateTime lastLoginDate, string SalerUserName,string DptName)
        {
            this.memID = memID;
            this.memName = memName;
            this.addressP = address_P;
            this.addressC = address_C;
            this.maxPosNum = MaxPosNum;
            this.maxResumeNum = maxResumeNum;
            this.beginValidDate = beginValidDate;
            this.endValidDate = endValidDate;
            this.memberClass = memberClass;
            this.lastLoginDate = lastLoginDate;
            this.salerName = SalerUserName;
            dptName= DptName;
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
        public string MemberClassName
        {
            get
            {
                return NameOtherProvider.GetMemberClassName(MemberClass);
            }
        }
        public short MemberClass
        {
            get { return memberClass; }
            set { memberClass = value; }
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
        public int MaxPosNum
        {
            get { return maxPosNum; }
            set { maxPosNum = value; }
        }
        public int MaxResumeNum
        {
            get { return maxResumeNum; }
            set { maxResumeNum = value; }
        }
        public string AddressStrName
        {
            get
            {
                return NameProvider.GetProvinceName(AddressP) + " " + NameProvider.GetCityName(AddressC);
            }
        }
        public DateTime LastLoginDate
        {
            get { return lastLoginDate; }
            set { lastLoginDate = value; }
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

        public string SalerName
        {
            get { return salerName; }
            set { salerName = value; }
        }
        public string DptName
        {
            get { return dptName; }
            set { dptName = value; }
        }
        public string ValidDateStr
        {
            get { return BeginValidDate.ToString("d") + "/" + EndValidDate.ToString("d"); }
        }
        public string LastLoginDateStr
        {
            get
            {
                return lastLoginDate.ToString("g");
            }
        }
    }
}
