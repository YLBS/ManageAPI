using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.TalentManagement
{
    /// <summary>
    /// 简历手机号码无效记录
    /// </summary>
    public class InAvailableMobileInfo
    {
        public int Id
        {
            get;
            set;
        }

        public int MyUserId
        {
            get;
            set;
        }

        public string PerName
        {
            get;
            set;
        }

        public int MemId
        {
            get;
            set;
        }

        public string MemName
        {
            get;
            set;
        }

        public string DisabledPhone
        {
            get;
            set;
        }

        public DateTime SetDate
        {
            get;
            set;
        }

        public string PhoneNow
        {
            get;
            set;
        }

        public bool HasDealWith
        {
            get;
            set;
        }
        public int DisabledList
        {
            get;
            set;
        }

        public string Contact
        {
            get;
            set;
        }
        public string DisabledListName
        {
            get
            {

                switch (DisabledList)
                {
                    case 1:
                        return "无人接听";
                    case 2:
                        return "不是本人";
                    case 3:
                        return "号码为空号";
                    case 4:
                        return "号码已过期";
                    case 5:
                        return "号码有误";
                    case 6:
                        return "其它原因";
                    default:
                        return "未设置原因";

                }
            }
        }
    }
}
