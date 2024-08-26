using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.SalesDepartment
{
    public class WxPusRecordInfo
    {
        private int _memId;
        private string _memName;
        private string _context;
        private DateTime _sendDateTime;
        private string _result;
        private int _sendType;
        private int _eplId;
        private string _eplName;
        public WxPusRecordInfo(int memID, string memName, string WxPushContext, DateTime SendDateTime, string Result, int SendType, int eplId, string eplName)
        {
            _memId = memID;
            _memName = memName;
            _context = WxPushContext;
            _sendDateTime = SendDateTime;
            _result = Result;
            _sendType = SendType;
            _eplId = eplId;
            _eplName = eplName;
        }
        public int MemId
        {
            get { return _memId;}
            set { _memId = value; }
        } 
        public string MemName
        {
            get { return _memName; }
            set { _memName = value; }
    }

        public string SendContext
        {
            get { return _context; }
            set { _context = value; }
        }

        public DateTime SendTime
        {
            get { return _sendDateTime; }
            set { _sendDateTime = value; }
        }
        public string SendResult
        {
            get { return _result; }
            set { _result = value; }
        }
        public int SendType
        {
            get { return _sendType; }
            set { _sendType = value; }
        }
        public string SendTypeStr
        {
            get
            {
                if (SendType == 1)
                {
                    return "系统发送";
                }
                if (SendType == 2)
                {
                    return "手动发送";
                }
                return "";
            }
        }

        public int EplId
        {
            get { return _eplId; }
            set { _eplId = value; }
        }
        public string SendEplName 
        {
            get { return _eplName; }
            set { _eplName = value; }

        } 
    }
}
