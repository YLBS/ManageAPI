using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.SalesDepartment
{
    /// <summary>
    /// 企业招聘效果
    /// </summary>
    public class EffectCompanyAdInfo
    {
        #region Member Variables
        private int id;
        private int memID;
        private string className;
        private int visitCount;
        private DateTime beginDate;
        private int validDay;
        private bool disabled;
        #endregion

        #region Constructors

        public EffectCompanyAdInfo(int id, int memID, string className, int visitCount, DateTime beginDate, int validDay, bool disabled)
        {
            this.id = id;
            this.memID = memID;
            this.className = className;
            this.visitCount = visitCount;
            this.beginDate = beginDate;
            this.validDay = validDay;
            this.disabled = disabled;
        }
        #endregion

        #region Public Properties

        public int ID
        {
            get { return id; }
        }

        public int MemID
        {
            get { return memID; }
        }

        public string ClassName
        {
            get { return className; }
        }

        public int VisitCount
        {
            get { return visitCount; }
        }

        public string BeginDate
        {
            get { return beginDate.ToString("d"); }
        }

        public string EndDate
        {
            get { return beginDate.AddDays(ValidDay).ToString("d"); }
        }

        public string OnlineMemo
        {
            get { return DateTime.Now > beginDate.AddDays(ValidDay) ? "下线" : "在线"; }
        }

        public int ValidDay
        {
            get { return validDay; }
        }

        public bool Disabled
        {
            get { return disabled; }
        }
        #endregion
    }
}
