

using System.Globalization;

namespace Model.SalesDepartment
{
    public class CompanyServiceDateInfo
    {
        private int _memId;
        private string _memName;
        private int _addressP;
        private int _addressC;
        private short _memberClass;
        private DateTime _beginValidDate;
        private DateTime _endValidDate;
        private int _restDateCount;

        public CompanyServiceDateInfo() { }

        public CompanyServiceDateInfo(int memId, string memName, int Address_P, int Address_C, short MemberClass, DateTime beginValidDate, DateTime endValidDate, int restDateCount)
        {
            _memId = memId;
            _memName = memName;
            _addressP = Address_P;
            _addressC = Address_C;
            _memberClass = MemberClass;
            _beginValidDate = beginValidDate;
            _endValidDate = endValidDate;
            _restDateCount = restDateCount;
        }

        public int MemID
        {
            set { _memId = value; }
            get { return _memId; }
        }

        public string MemName
        {
            set { _memName = value; }
            get { return _memName; }
        }

        public int AddressP
        {
            set { _addressP = value; }
            get { return _addressP; }
        }

        public int AddressC
        {
            set { _addressC = value; }
            get { return _addressC; }
        }

        public string AddressName
        {
            get { return Goodjob.Common.NameProvider.GetProvinceName(_addressP) + "-" + Goodjob.Common.NameProvider.GetCityName(_addressC); }
        }

        public short MemberClass
        {
            set { _memberClass = value; }
            get { return _memberClass; }
        }

        public string MemberClassName
        {
            get { return Goodjob.Common.NameOtherProvider.GetMemberClassName(_memberClass); }
        }

        public string BeginValidDate
        {
            set { _beginValidDate = Convert.ToDateTime(value); }
            get { return _beginValidDate.ToString("yyyy-MM-dd"); }
        }

        public string EndValidDate
        {
            set { _endValidDate = Convert.ToDateTime(value); }
            get { return _endValidDate.ToString("yyyy-MM-dd"); }
        }

        public int RestDateCount
        {
            set { _restDateCount = value; }
            get { return _restDateCount; }
        }
    }
}
