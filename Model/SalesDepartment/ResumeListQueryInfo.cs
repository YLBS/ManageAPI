using Goodjob.Common;

namespace Model.SalesDepartment
{
    /// <summary>
    /// 字段过多，待优化
    /// </summary>
    public class ResumeListQueryInfo
    {
        private int resumeID;

        //ResumeStatusInfo
        private byte photoFlag;
        private byte engResumeFlag;
        private byte contactStatus;

        //ResumeQueryInfo
        private string perName;
        private byte sex;
        private DateTime birthday;
        private int hometownP;
        private int hometownC;
        private int locationP;
        private int locationC;
        private int degreeID;
        private string schoolName;
        private string speciality;
        private int workedYear;
        private string lastPosName;
        private DateTime updateDate;

        private DateTime registerDate;


        public ResumeListQueryInfo(int myUserId, byte photoFlag, byte engResumeFlag, byte contactStatus, string perName, byte sex, DateTime birthday, int hometown_P, int hometown_C, int location_P, int location_C, int degreeID, string schoolName, string speciality, int workedYear, string lastPosName, DateTime updateDate, DateTime RegisterDate)
        {
            this.resumeID = myUserId;
            this.photoFlag = photoFlag;
            this.engResumeFlag = engResumeFlag;
            this.contactStatus = contactStatus;
            this.perName = perName;
            this.sex = sex;
            this.birthday = birthday;
            this.hometownP = hometown_P;
            this.hometownC = hometown_C;
            this.locationP = location_P;
            this.locationC = location_C;
            this.degreeID = degreeID;
            this.schoolName = schoolName;
            this.speciality = speciality;
            this.workedYear = workedYear;
            this.lastPosName = lastPosName;
            this.updateDate = updateDate;
            this.registerDate = RegisterDate;
        }

        #region 没有引用了，20240902
        /*
        public ResumeListQueryInfo(int resumeID, byte photoFlag, byte engResumeFlag, byte contactStatus, string perName, byte sex, DateTime birthday, int hometownP, int hometownC, int locationP, int locationC, int degreeID, string schoolName, string speciality, int workedYear, string lastPosName, DateTime updateDate, string registerFrom, string extranteID)
        {
            this.resumeID = resumeID;
            this.photoFlag = photoFlag;
            this.engResumeFlag = engResumeFlag;
            this.contactStatus = contactStatus;
            this.perName = perName;
            this.sex = sex;
            this.birthday = birthday;
            this.hometownP = hometownP;
            this.hometownC = hometownC;
            this.locationP = locationP;
            this.locationC = locationC;
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
        #endregion

        
        public int ResumeID
        {
            get { return resumeID; }
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
            get { return photoFlag; }
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
            get { return registerDate; }
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
                string cityName = NameProvider.GetCityName(hometownC);
                return NameProvider.GetProvinceName(hometownP) + (string.IsNullOrEmpty(cityName) ? string.Empty : "-" + cityName);
            }
        }

        public string Location
        {
            get
            {
                string cityName = NameProvider.GetCityName(locationC);
                return NameProvider.GetProvinceName(locationP) + (string.IsNullOrEmpty(cityName) ? string.Empty : "-" + cityName);
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
    }
}
