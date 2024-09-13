using Goodjob.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ResumeSearch
{
    /// <summary>
    /// 11个构造方法。。。
    /// </summary>
    public class ResumeListInfo
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

        private DateTime birthday;
        private short maritalStatus;
        private short degreeID;
        private string selfDescription;
        private short workedYear;
        private short workedMonth;
        private int town;
        private string jobSeeking;
        private string jobFunctionName;

        private bool isCommend;

        private bool isqdStatus;

        private string mobileNum;

        public int resumeStatus;

        public DateTime lastLoginDate;

        public short workWrite;

        public string serverDate;

        public int resumesource;

        public ResumeListInfo()
        { }

        public ResumeListInfo(int myUserID, string perName, short sex, string email, int location_P, int location_C, DateTime registerDate, DateTime updateDate, short allWrite, short checkFlag, int resumesource)
        {
            this.myUserID = myUserID;
            this.perName = perName;
            this.sex = sex;
            this.email = email;
            this.location_P = location_P;
            this.location_C = location_C;
            this.registerDate = registerDate;
            this.updateDate = updateDate;
            this.allWrite = allWrite;
            this.checkFlag = checkFlag;
            this.resumesource = resumesource;
        }

        public ResumeListInfo(int myUserID, string perName, short sex, string email, int location_P, int location_C, DateTime registerDate, DateTime updateDate, short allWrite, short checkFlag, string mobileNum)
        {
            this.myUserID = myUserID;
            this.perName = perName;
            this.sex = sex;
            this.email = email;
            this.location_P = location_P;
            this.location_C = location_C;
            this.registerDate = registerDate;
            this.updateDate = updateDate;
            this.allWrite = allWrite;
            this.checkFlag = checkFlag;
            this.mobileNum = mobileNum;
        }

        public ResumeListInfo(int myUserID, string perName, short sex, DateTime birthday, short maritalStatus, int location_P, int location_C, short degreeID, string selfDescription, short workedYear, short workedMonth, int town, string jobSeeking, string jobFunctionName, DateTime updateDate)
        {
            this.myUserID = myUserID;
            this.perName = perName;
            this.sex = sex;
            this.birthday = birthday;
            this.maritalStatus = maritalStatus;
            this.location_P = location_P;
            this.location_C = location_C;
            this.degreeID = degreeID;
            this.selfDescription = selfDescription;
            this.workedYear = workedYear;
            this.workedMonth = workedMonth;
            this.town = town;
            this.jobSeeking = jobSeeking;
            this.jobFunctionName = jobFunctionName;
            this.updateDate = updateDate;
        }
        public ResumeListInfo(int myUserID, string perName, short sex, DateTime birthday, short maritalStatus, int location_P, int location_C, short degreeID, string selfDescription, short workedYear, short workedMonth, int town, string jobSeeking, string jobFunctionName, DateTime updateDate, DateTime registerDate, bool isCommend)
        {
            this.myUserID = myUserID;
            this.perName = perName;
            this.sex = sex;
            this.birthday = birthday;
            this.maritalStatus = maritalStatus;
            this.location_P = location_P;
            this.location_C = location_C;
            this.degreeID = degreeID;
            this.selfDescription = selfDescription;
            this.workedYear = workedYear;
            this.workedMonth = workedMonth;
            this.town = town;
            this.jobSeeking = jobSeeking;
            this.jobFunctionName = jobFunctionName;
            this.updateDate = updateDate;
            this.registerDate = registerDate;
            this.isCommend = isCommend;
        }

        //18个参数
        public ResumeListInfo(int myUserID, string perName, short sex, DateTime birthday, short maritalStatus, int location_P, int location_C, short degreeID, string selfDescription, short workedYear, short workedMonth, int town, string jobSeeking, string jobFunctionName, DateTime updateDate, DateTime registerDate, bool isCommend, int resumeStatus)
        {
            this.myUserID = myUserID;
            this.perName = perName;
            this.sex = sex;
            this.birthday = birthday;
            this.maritalStatus = maritalStatus;
            this.location_P = location_P;
            this.location_C = location_C;
            this.degreeID = degreeID;
            this.selfDescription = selfDescription;
            this.workedYear = workedYear;
            this.workedMonth = workedMonth;
            this.town = town;
            this.jobSeeking = jobSeeking;
            this.jobFunctionName = jobFunctionName;
            this.updateDate = updateDate;
            this.registerDate = registerDate;
            this.isCommend = isCommend;
            this.resumeStatus = resumeStatus;
        }
        //19个
        public ResumeListInfo(int myUserID, string perName, short sex, DateTime birthday, short maritalStatus, int location_P, int location_C, short degreeID, string selfDescription, short workedYear, short workedMonth, int town, string jobSeeking, string jobFunctionName, DateTime updateDate, DateTime registerDate, bool isCommend, int resumeStatus, DateTime lastLoginDate, short workWrite)
        {
            this.myUserID = myUserID;
            this.perName = perName;
            this.sex = sex;
            this.birthday = birthday;
            this.maritalStatus = maritalStatus;
            this.location_P = location_P;
            this.location_C = location_C;
            this.degreeID = degreeID;
            this.selfDescription = selfDescription;
            this.workedYear = workedYear;
            this.workedMonth = workedMonth;
            this.town = town;
            this.jobSeeking = jobSeeking;
            this.jobFunctionName = jobFunctionName;
            this.updateDate = updateDate;
            this.registerDate = registerDate;
            this.isCommend = isCommend;
            this.resumeStatus = resumeStatus;
            this.lastLoginDate = lastLoginDate;
            this.workWrite = workWrite;
        }

        //20个
        public ResumeListInfo(int myUserID, string perName, short sex, DateTime birthday, short maritalStatus, int location_P, int location_C, short degreeID, string selfDescription, short workedYear, short workedMonth, int town, string jobSeeking, string jobFunctionName, DateTime updateDate, DateTime registerDate, bool isCommend, int resumeStatus, DateTime lastLoginDate, short workWrite, string serverDate)
        {
            this.myUserID = myUserID;
            this.perName = perName;
            this.sex = sex;
            this.birthday = birthday;
            this.maritalStatus = maritalStatus;
            this.location_P = location_P;
            this.location_C = location_C;
            this.degreeID = degreeID;
            this.selfDescription = selfDescription;
            this.workedYear = workedYear;
            this.workedMonth = workedMonth;
            this.town = town;
            this.jobSeeking = jobSeeking;
            this.jobFunctionName = jobFunctionName;
            this.updateDate = updateDate;
            this.registerDate = registerDate;
            this.isCommend = isCommend;
            this.resumeStatus = resumeStatus;
            this.lastLoginDate = lastLoginDate;
            this.workWrite = workWrite;
            this.serverDate = serverDate;
        }

        //22个
        public ResumeListInfo(int myUserID, string perName, short sex, DateTime birthday, short maritalStatus, int location_P, int location_C, short degreeID, string selfDescription, short workedYear, short workedMonth, int town, string jobSeeking, string jobFunctionName, DateTime updateDate, DateTime registerDate, bool isCommend, int resumeStatus, DateTime lastLoginDate, short workWrite, string serverDate, string email, string mobileNum)
        {
            this.myUserID = myUserID;
            this.perName = perName;
            this.sex = sex;
            this.birthday = birthday;
            this.maritalStatus = maritalStatus;
            this.location_P = location_P;
            this.location_C = location_C;
            this.degreeID = degreeID;
            this.selfDescription = selfDescription;
            this.workedYear = workedYear;
            this.workedMonth = workedMonth;
            this.town = town;
            this.jobSeeking = jobSeeking;
            this.jobFunctionName = jobFunctionName;
            this.updateDate = updateDate;
            this.registerDate = registerDate;
            this.isCommend = isCommend;
            this.resumeStatus = resumeStatus;
            this.lastLoginDate = lastLoginDate;
            this.workWrite = workWrite;
            this.serverDate = serverDate;
            this.email = email;
            this.mobileNum = mobileNum;
        }

        public ResumeListInfo(int myUserID, string perName, DateTime updateDate, int resumeStatus, DateTime lastLoginDate, string serverDate, string email, string mobileNum, DateTime registerDate)
        {
            this.myUserID = myUserID;
            this.perName = perName;
            this.updateDate = updateDate;
            this.resumeStatus = resumeStatus;
            this.lastLoginDate = lastLoginDate;
            this.serverDate = serverDate;
            this.email = email;
            this.mobileNum = mobileNum;
            this.registerDate = registerDate;
        }

        public ResumeListInfo(int myUserID, string perName, short sex, string email, int location_P, int location_C, DateTime registerDate, DateTime updateDate, short allWrite, short checkFlag, string mobileNum, bool isqdStatus)
        {
            this.myUserID = myUserID;
            this.perName = perName;
            this.sex = sex;
            this.email = email;
            this.location_P = location_P;
            this.location_C = location_C;
            this.registerDate = registerDate;
            this.updateDate = updateDate;
            this.allWrite = allWrite;
            this.checkFlag = checkFlag;
            this.mobileNum = mobileNum;
            this.isqdStatus = isqdStatus;
        }

        #region Public Properties

        public bool IsqdStatus
        {
            get { return isqdStatus; }
            set { isqdStatus = value; }
        }

        public bool IsCommend
        {
            get { return isCommend; }
            set { isCommend = value; }
        }

        public int ResumeStatus
        {
            get { return resumeStatus; }
            set { resumeStatus = value; }

        }

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

        public DateTime RegisterDate_Sub1
        {
            get { return registerDate; }
            set { registerDate = value; }
        }
        public string RegisterDate_Sub2 { get; set; }

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

        public string MaritalName
        {
            get { return NameProvider.GetMaritalName(Convert.ToInt32(maritalStatus)); }
        }
        public string DegreeName
        {
            get { return NameProvider.GetDegreeName(Convert.ToInt32(degreeID)); }
        }
        public int Age
        {
            get
            {
                return System.DateTime.Today.Year - birthday.Year;
            }
        }
        public short WorkedYear
        {
            get { return workedYear; }
            set { workedYear = value; }
        }
        public string SelfDescription
        {
            get { return selfDescription; }
            set { selfDescription = value; }
        }

        public string JobFunctionName
        {
            get { return jobFunctionName; }
            set { jobFunctionName = value; }
        }
        public string JobSeeking
        {
            get { return jobSeeking; }
            set { jobSeeking = value; }
        }
        //public DateTime LastLoginDate
        //{
        //    get { return lastLoginDate; }
        //    set { lastLoginDate = value; }
        //} get { return updateDate.ToString("d"); }  

        public string LastLoginDate
        {
            get
            {
                if (lastLoginDate > registerDate)
                {
                    return lastLoginDate.ToString("d");
                }
                else
                {
                    return "未登录";
                }
            }
        }

        public string ServerDate
        {
            get { return serverDate; }
        }

        public string HopeJob
        {
            get
            {
                if (!string.IsNullOrEmpty(JobSeeking))
                {
                    return JobFunctionName + "(简历标题：" + JobSeeking + ")";
                }
                else
                {
                    return JobFunctionName;
                }
            }
        }

        public int IsFlag
        {
            get
            {
                if (!string.IsNullOrEmpty(SelfDescription) && !string.IsNullOrEmpty(PerName) && !string.IsNullOrEmpty(HopeJob))
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        }

        public bool IsCommends
        {
            get
            {
                if (!IsCommend && !string.IsNullOrEmpty(SelfDescription) && !string.IsNullOrEmpty(PerName) && !string.IsNullOrEmpty(HopeJob))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public int MemLocation
        {
            get { return town; }
            set { town = value; }
        }



        public string LocationNews
        {
            get
            {
                string cityName = NameProvider.GetCityName(location_C);
                string townName = NameProvider.GetTownName(location_C, MemLocation);
                return NameProvider.GetProvinceName(location_P) + (string.IsNullOrEmpty(cityName) ? string.Empty : "-" + cityName + (string.IsNullOrEmpty(townName) ? string.Empty : "-" + townName));
            }
        }

        public short WorkWrite
        {
            get { return workWrite; }
            set { workWrite = value; }
        }

        public string MyLoginString { get; set; }

        public string ResumeSource
        {
            get
            {
                string value = "";
                switch (resumesource)
                {
                    case 1:
                        value = "PC";
                        break;

                    case 2:
                        value = "触摸";
                        break;

                    case 3:
                        value = "安卓";
                        break;
                    case 4:
                        value = "苹果";
                        break;
                    case 5:
                        value = "小程序";
                        break;

                    case 7:
                        value = "百度";
                        break;
                    default:
                        value = "N/A";
                        break;
                }
                return value;
            }
        }
        /// <summary>
        /// 去电人员ID
        /// </summary>
        public int qdUserID { get; set; }
        /// <summary>
        /// 去电人员姓名
        /// </summary>
        public string qdUserName
        {
            get
            {
                string value = "";
                switch (qdUserID)
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
        #endregion
    }

}
