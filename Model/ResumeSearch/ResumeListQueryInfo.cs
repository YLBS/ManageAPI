using Goodjob.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ResumeSearch
{
    /// <summary>
    /// 与 SalesDepartment的不同，SalesDepartment的不需要太多字段，待优化
    /// </summary>
    public class ResumeListQueryInfo
    {

        private int myUserID;

        //ResumeStatusInfo
        private byte PhotoDisplay;
        private byte engResumeFlag;
        private byte contactStatus;

        //ResumeQueryInfo
        private string perName;
        private byte sex;
        private DateTime birthday;
        private int hometown_P;
        private int hometown_C;
        private int location_P;
        private int location_C;
        private int degreeID;
        private string schoolName;
        private string speciality;
        private int workedYear;
        private string lastPosName;
        private DateTime updateDate;
        public string registerFrom;
        public string extranteID;

        public ResumeListQueryInfo(){}
        public ResumeListQueryInfo(int resumeID, byte photoFlag, byte engResumeFlag, byte contactStatus, string perName, byte sex, DateTime birthday, int hometownP, int hometownC, int locationP, int locationC, int degreeID, string schoolName, string speciality, int workedYear, string lastPosName, DateTime updateDate)
        {
            this.myUserID = resumeID;
            this.PhotoDisplay = photoFlag;
            this.engResumeFlag = engResumeFlag;
            this.contactStatus = contactStatus;
            this.perName = perName;
            this.sex = sex;
            this.birthday = birthday;
            this.hometown_P = hometownP;
            this.hometown_C = hometownC;
            this.location_P = locationP;
            this.location_C = locationC;
            this.degreeID = degreeID;
            this.schoolName = schoolName;
            this.speciality = speciality;
            this.workedYear = workedYear;
            this.lastPosName = lastPosName;
            this.updateDate = updateDate;
        }
        /*

        外部简历推荐使用自动映射
        /// <summary>
        /// 外部简历推荐用
        /// </summary>
        public ResumeListQueryInfo(int myUserID, byte PhotoDisplay, byte EngResumeFlag, byte ContactStatus, string perName, byte sex, DateTime birthday, int Hometown_P, int hometown_C, int location_P, int location_C, int degreeID, string schoolName, string speciality, int workedYear, string lastPosName, DateTime updateDate, string registerFrom, string extranteID)
        {
            this.myUserID = myUserID;
            this.PhotoDisplay = PhotoDisplay;
            this.engResumeFlag = EngResumeFlag;
            this.contactStatus = ContactStatus;
            this.perName = perName;
            this.sex = sex;
            this.birthday = birthday;
            this.hometown_P = Hometown_P;
            this.hometown_C = hometown_C;
            this.location_P = location_P;
            this.location_C = location_C;
            this.degreeID = degreeID;
            this.schoolName = schoolName;
            this.speciality = speciality;
            this.workedYear = workedYear;
            this.lastPosName = lastPosName;
            this.updateDate = updateDate;
            this.registerFrom = registerFrom;
            this.extranteID = extranteID;
        }

        */

        public int ResumeID
        {
            get { return myUserID; }
        }

        public byte EngResumeFlag
        {
            get { return engResumeFlag; }
        }

        public byte ContactStatus
        {
            get { return contactStatus; }
        }

        public byte PhotoFlag
        {
            get { return PhotoDisplay; }
        }

        public string PerName
        {
            get { return perName; }
        }

        public string SchoolName
        {
            get { return schoolName; }
        }

        public string Speciality
        {
            get { return speciality; }
        }

        public int WorkedYear
        {
            get { return workedYear; }
        }

        public string LastPosName
        {
            get { return lastPosName; }
        }

        public string UpdateDate
        {
            get { return updateDate.ToString("g"); }
        }

        public string RegisterDateString
        {
            get { return RegisterDate.ToString("g"); }
        }
        public DateTime RegisterDate
        {
            get; set;
        }

        public string SexName
        {
            get { return NameProvider.GetSexName(sex); }
        }

        public int Age
        {
            get
            {
                return System.DateTime.Today.Year - birthday.Year;
            }
        }

        public string DegreeName
        {
            get { return NameProvider.GetDegreeName(degreeID); }
        }

        public string HomeTown
        {
            get
            {
                string cityName = NameProvider.GetCityName(hometown_C);
                return NameProvider.GetProvinceName(hometown_P) + (string.IsNullOrEmpty(cityName) ? string.Empty : "-" + cityName);
            }
        }

        public string Location
        {
            get
            {
                string cityName = NameProvider.GetCityName(location_C);
                return NameProvider.GetProvinceName(location_P) + (string.IsNullOrEmpty(cityName) ? string.Empty : "-" + cityName);
            }
        }

        public string RegisterFrom
        {
            get
            {
                return GetJLName(Convert.ToInt32(registerFrom));
            }
        }



        public string ExtranteID
        {
            get
            {
                return extranteID;
            }
        }

        public static string[,] jianli = new string[,]
        {
                                        {"0","无"},
                                        {"1","赶集"},
                                        {"2","58"},
                                        {"3","51job"},
                                        {"4","南方/番禺人才网"},
                                        {"5","智联招聘"},
                                        {"6","门店"},
                                        {"7","现场招聘"},
                                        {"8","黄阁镇人社局"},
                                        {"9","达内教育培训"},
                                        {"10","未知10"},
                                        {"11","未知11"},
                                        {"12","boss"},
                                        {"13","智联"},
                                        {"14","前程"},
                                        {"100","其他"}
        };

        public string GetJLName(int BigJobID)
        {
            for (int i = 0; i < jianli.GetLength(0); i++)
            {
                if (BigJobID == Convert.ToInt32(jianli.GetValue(i, 0)))
                {
                    return jianli[i, 1];
                }

            }
            return string.Empty;
        }

        /// <summary>
        /// 求职者回复情况
        /// </summary>
        public string MyResponseStatus { get; set; }
        public string JobSeeking { get; set; }
        public string ContactPhone { get; set; }
        public string MobileNum { get; set; }
    }
}
