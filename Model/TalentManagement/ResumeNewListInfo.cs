using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Goodjob.Common;

namespace Model.TalentManagement
{
    public class ResumeNewListInfo
    {

        private int myUserID;
        private string perName;
        private short sex;
        private string email;
        private int location_P;
        private int location_C;
        private DateTime registerDate;
        private DateTime updateDate;
        private short allWrite;
        private short checkFlag;

        private string mobileNum;
        private string emaiFiag;
        private int emaiSend;
        private int phoneSend;
        
        private int qdType;
        private bool isdqStatus;
        private int qdUserId;
        private string lastLoginDate;
        private DateTime firstSendTime;


        public ResumeNewListInfo()
        { }
        

        public ResumeNewListInfo(int myUserID, string perName, string mobileNum, short sex, int location_P, int location_C, DateTime registerDate, DateTime updateDate, int emaiSend, int phoneSend, string emaiFiag, bool isdqStatus, int qdUserId, int qdType)
        {
            this.myUserID = myUserID;
            this.perName = perName;
            this.mobileNum = mobileNum;
            this.sex = sex;
            this.location_P = location_P;
            this.location_C = location_C;
            this.registerDate = registerDate;
            this.updateDate = updateDate;
            this.emaiSend = emaiSend;
            this.phoneSend = phoneSend;
            this.emaiFiag = emaiFiag;
            this.isdqStatus = isdqStatus;
            this.qdUserId = qdUserId;
            this.qdType = qdType;
        }
        

        #region Public Properties

        public int MyUserID
        {
            get { return myUserID; }
            set { myUserID = value; }
        }

        public string PerName
        {
            get
            {
                if (string.IsNullOrEmpty(perName))
                { return "-未填-"; }
                return perName;
            }
            set { perName = value; }
        }

        public short Sex
        {
            get { return sex; }
            set { sex = value; }
        }
        public string SexName
        {
            get { return NameProvider.GetSexName(Convert.ToInt32(Sex)); }
        }

        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        public int Location_P
        {
            get { return location_P; }
            set { location_P = value; }
        }

        public int Location_C
        {
            get { return location_C; }
            set { location_C = value; }
        }

        public string Location
        {
            get { return NameProvider.GetProvinceName(location_P) + "-" + NameProvider.GetCityName(location_C); }
        }

        public string RegisterDate
        {
            get { return registerDate.ToString("d"); }
        }

        public string UpdateDate
        {
            get { return updateDate.ToString("d"); }
        }

        public short AllWrite
        {
            get { return allWrite; }
            set { allWrite = value; }
        }
        public string AllWriteName
        {
            get { return NameOtherProvider.GetResumeAllWriteName(allWrite); }
        }
        public short CheckFlag
        {
            get { return checkFlag; }
            set { checkFlag = value; }
        }
        public string CheckName
        {
            get { return NameOtherProvider.GetResumeCheckFlagName(checkFlag); }
        }

        public string MobileNum
        {
            get { return this.mobileNum; }
        }
        public string EmaiFiag
        {
            get { return emaiFiag; }
            set { emaiFiag = value; }
        }

        public int EmaiSend
        {
            get { return emaiSend; }
            set { emaiSend = value; }
        }

        public int PhoneSendCount
        {
            get { return phoneSend; }
            set { phoneSend = value; }
        }

        #region 613add
        public string QdResult
        {
            get
            {
                switch (qdType)
                {
                    case 1:
                        return "已联系已推荐";
                    case 2:
                        return "关机";
                    case 3:
                        return "空号";
                    case 4:
                        return "拒接";
                    case 5:
                        return "隐藏简历";
                    case 6:
                        return "待删除";
                    case 7:
                        return "未接停";
                    default:
                        return "";
                }
            }
        }

        public bool IsdqStatus
        {
            get { return isdqStatus; }
            set { isdqStatus = value; }
        }

        public int QdUserId
        {
            get { return qdUserId; }
            set { qdUserId = value; }
        }
        public string qdUserName
        {
            get
            {
                string value = "";
                switch (QdUserId)
                {
                    case 0:
                        value = "未去电";
                        break;
                    case 458:
                        value = "杨镇隆";
                        break;

                    case 1446:
                        value = "何俊毅";
                        break;

                    case 1586:
                        value = "叶焯雯";
                        break;
                    case 1942:
                        value = "黄韵萍";
                        break;
                    case 1581:
                        value = "王芳";
                        break;
                    case 53:
                        value = "吴智芬";
                        break;
                    case 739:
                        value = "张秋松";
                        break;
                    case 2051:
                        value = "何洁敏";
                        break;
                    case 2053:
                        value = "许志球";
                        break;

                    case 1943:
                        value = "高岚";
                        break;
                    case 1133:
                        value = "陈婉冬";
                        break;
                    case 1570:
                        value = "梁凤平";
                        break;
                    case 1697:
                        value = "黄嘉琪";
                        break;
                    case 1763:
                        value = "梁议文";
                        break;
                    case 1772:
                        value = "陈铭恒";
                        break;
                }
                return value;
            }
        }

        public string SourceName { get; set; }
        #endregion


        public string LastLoginDate
        {
            get { return lastLoginDate; }
            set { lastLoginDate = value; }
        }
        public DateTime FirstSendTime
        {
            get { return firstSendTime; }
            set { firstSendTime = value; }
        }
        
        #endregion
    }
}
