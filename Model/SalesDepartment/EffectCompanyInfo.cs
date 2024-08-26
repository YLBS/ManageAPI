using Goodjob.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.SalesDepartment
{
    /// <summary>
    /// 公共企业库，企业信息
    /// </summary>
    public class EffectCompanyInfo
    {
        #region Member Variables
        private int memID;
        private string memName;
        private short calling;
        private int addressP;
        private int addressC;
        private bool iscommend;
        private bool hasPage;
        private DateTime updateDate;
        #endregion

        #region Constructors
        public EffectCompanyInfo() { }

        public EffectCompanyInfo(int MemId, string memName, short calling, int AddressP, int AddressC, bool iscommend, bool hasPage, DateTime updateDate)
        {
            this.memID = MemId;
            this.memName = memName;
            this.calling = calling;
            this.addressP = AddressP;
            this.addressC = AddressC;
            this.iscommend = iscommend;
            this.hasPage = hasPage;
            this.updateDate = updateDate;
        }
        #endregion

        #region Public Properties
        public int MemID
        {
            get { return memID; }
        }

        public string MemName
        {
            get { return memName; }
        }

        public string CallingName
        {
            get { return NameProvider.GetIndustryName(calling); }
        }

        public string AddressName
        {
            get { return NameProvider.GetProvinceName(addressP) + " " + NameProvider.GetCityName(addressC); }
        }

        public string IsCommend
        {
            get { return iscommend ? "有" : "没有"; }
        }

        public string HasPage
        {
            get { return hasPage ? "有" : "没有"; }
        }

        public string UpdateDate
        {
            get { return updateDate.ToString("d"); }
        }
        #endregion
    }
}
