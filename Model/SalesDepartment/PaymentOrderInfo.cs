using Goodjob.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.SalesDepartment
{
    public class PaymentOrderInfo
    {
        private int orderID;
        private int memID;
        private int payMoney;
        private string description;
        private string payPerson;
        private DateTime payDate;
        private short doFlag;
        private string memName;

        public PaymentOrderInfo()
        {

        }
        public PaymentOrderInfo(int orderId, int memId, int payMoney, string Description, string payPerson, DateTime payDate, short doFlag, string memName)
        {
            this.orderID = orderId;
            this.memID = memId;
            this.payMoney = payMoney;
            this.description = Description;
            this.payPerson = payPerson;
            this.payDate = payDate;
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
        public int PayMoney
        {
            get { return payMoney; }
            set { payMoney = value; }
        }
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        public string DescriptionStr
        {
            get
            {
                if (Description.Length > 15)
                    return Description.Substring(0, 15) + "...";
                return Description;
            }
        }
        public string PayPerson
        {
            get { return payPerson; }
            set { payPerson = value; }
        }
        public string PayDate
        {
            get { return payDate.ToString("g"); }
        }
        public short DoFlag
        {
            get { return doFlag; }
            set { doFlag = value; }
        }
        public string DoFlagName
        {
            get { return NameOtherProvider.GetPaymentDoFlagName(DoFlag); }
        }
        public string MemName
        {
            get { return memName; }
            set { memName = value; }
        }
    }
}
