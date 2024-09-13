using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.SalesDepartment
{
    public class UserComplaint
    {
        // 私有变量
        private int _id;
        private int _myUserId;
        private string _perName;
        private int _memId;
        private string _memName;
        private int _posId;
        private string _posName;
        private int _salerUserId;
        private string _salerUserName;
        private int _type;
        private string _content;
        private DateTime _addTime;
        private bool _hasDealWith;
        private string _processingDetail;
        private string _email;
        private int _countToal;

        // 构造器
        public UserComplaint() { }

        public UserComplaint(int id, int myUserId, string perName, int memId, string memName, int posId, string posName, int salerUserId, string salerUserName, int type, string content, DateTime addTime, bool hasDealWith)
        {
            this._id = id;
            this._myUserId = myUserId;
            this._perName = perName;
            this._memId = memId;
            this._memName = memName;
            this._posId = posId;
            this._posName = posName;
            this._salerUserId = salerUserId;
            this._salerUserName = salerUserName;
            this._type = type;
            this._content = content;
            this._addTime = addTime;
            this._hasDealWith = hasDealWith;
        }

        public UserComplaint(int id, int myUserId, string perName, int memId, string memName, int posId, string posName, int salerUserId, string salerUserName, int type, string content, DateTime addTime, bool hasDealWith, string processingDetail, string email)
        {
            this._id = id;
            this._myUserId = myUserId;
            this._perName = perName;
            this._memId = memId;
            this._memName = memName;
            this._posId = posId;
            this._posName = posName;
            this._salerUserId = salerUserId;
            this._salerUserName = salerUserName;
            this._type = type;
            this._content = content;
            this._addTime = addTime;
            this._hasDealWith = hasDealWith;
            this._processingDetail = processingDetail;
            this._email = email;
        }

        public UserComplaint(int memId, string memName, int posId, string posName, int salerUserId, string salerUserName, int countToal, DateTime addTime)
        {
            this._memId = memId;
            this._memName = memName;
            this._posId = posId;
            this._posName = posName;
            this._salerUserId = salerUserId;
            this._salerUserName = salerUserName;
            this._countToal = countToal;
            this._addTime = addTime;
        }

        // 公有变量
        public int ID
        {
            set { this._id = value; }
            get { return this._id; }
        }

        public int MyUserID
        {
            set { this._myUserId = value; }
            get { return this._myUserId; }
        }

        public string PerName
        {
            set { this._perName = value; }
            get { return this._perName; }
        }

        public int MemID
        {
            set { this._memId = value; }
            get { return this._memId; }
        }

        public string MemName
        {
            set { this._memName = value; }
            get { return this._memName; }
        }

        public int PosID
        {
            set { this._posId = value; }
            get { return this._posId; }
        }

        public string PosName
        {
            set { this._posName = value; }
            get { return this._posName; }
        }

        public int SalerUserID
        {
            set { this._salerUserId = value; }
            get { return this._salerUserId; }
        }

        public string SalerUserName
        {
            set { this._salerUserName = value; }
            get { return this._salerUserName; }
        }

        public int Type
        {
            set { this._type = value; }
            get { return this._type; }
        }

        public string TypeName
        {
            get
            {
                string typeName = string.Empty;
                switch (_type)
                {
                    case 1:
                        typeName = "该职位已过期";
                        break;
                    case 2:
                        typeName = "该职位信息不实";
                        break;
                    case 3:
                        typeName = "该职位企业已倒闭";
                        break;
                    case 4:
                        typeName = "该职位已经招到人";
                        break;
                    default:
                        typeName = "该类型不存在，请检查数据";
                        break;
                }
                return typeName;
            }
        }

        public string Content
        {
            set { this._content = value; }
            get { return this._content; }
        }

        public DateTime AddTime
        {
            set { this._addTime = value; }
            get { return this._addTime; }
        }

        public bool HasDealWith
        {
            set { this._hasDealWith = value; }
            get { return this._hasDealWith; }
        }

        public string ProcessingDetail
        {
            set { this._processingDetail = value; }
            get { return this._processingDetail; }
        }

        public string DealResult
        {
            get { return _hasDealWith ? "处理完毕" : "尚未处理"; }
        }

        public string Email
        {
            set { this._email = value; }
            get { return this._email; }
        }

        public string AddTimeStr
        {
            get { return this._addTime.ToShortDateString(); }
        }

        public int CountToal
        {
            set { this._countToal = value; }
            get { return this._countToal; }
        }

        public string DealResultStr
        {
            get
            {
                //return _hasDealWith ? "处理完毕" : "尚未处理"; 
                if (_hasDealWith)
                {
                    return "处理完毕";
                }
                else
                {
                    if (!string.IsNullOrEmpty(_processingDetail))
                    {
                        return "销售人员已处理";
                    }
                    else
                    {
                        return "尚未处理";
                    }
                }


            }
        }

    }
}
