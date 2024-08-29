using Goodjob.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.SalesDepartment
{
    public class ServiceOrderInfo
    {
        private int orderID;
        private int memID;
        private int itemID;
        private int validDay;
        private string detail;
        private string applyPerson;
        private DateTime applyDate;
        private short doFlag;
        private string memName;

        public ServiceOrderInfo() { }
        public ServiceOrderInfo(int orderID, int memID, int itemID, int validDay, string detail, string applyPerson, DateTime applyDate, short doFlag, string memName)
        {
            this.orderID = orderID;
            this.memID = memID;
            this.itemID = itemID;
            this.validDay = validDay;
            this.detail = detail;
            this.applyPerson = applyPerson;
            this.applyDate = applyDate;
            this.doFlag = doFlag;
            this.memName = memName;
        }
        public int OrderID
        {
            get { return orderID; }
            set { orderID = value; }
        }
        public int MemID
        {
            get { return memID; }
            set { memID = value; }
        }
        public int ItemID
        {
            get { return itemID; }
            set { itemID = value; }
        }
        public string ItemName
        {
            get { return NameOtherProvider.GetServiceItemName(ItemID); }
        }
        public int ValidDay
        {
            get { return validDay; }
            set { validDay = value; }
        }
        public string Detail
        {
            get { return detail; }
            set { detail = value; }
        }
        public string DetailStr
        {
            get
            {
                if (Detail.Length > 15)
                    return Detail.Substring(0, 15) + "...";
                return Detail;
            }
        }
        public string ApplyPerson
        {
            get { return applyPerson; }
            set { applyPerson = value; }
        }
        public string ApplyDate
        {
            get { return applyDate.ToString("g"); }
        }
        public short DoFlag
        {
            get { return doFlag; }
            set { doFlag = value; }
        }
        public string DoFlagName
        {
            get { return NameOtherProvider.GetServiceDoFlagName(DoFlag); }
        }
        public string MemName
        {
            get { return memName; }
            set { memName = value; }
        }
    }
}
