using Goodjob.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.TalentManagement
{
    /// <summary>
    /// 新注册待审核简历
    /// </summary>
    public class CheckingResumeInfo
    {
        #region Member Variables
        private int id;
        private int myUserID;
        private string perName;
        private int sex;
        private int age;
        private int location_P;
        private int location_C;
        private DateTime registerDate;
        private DateTime updateDate;
        private short checkFlag;
        #endregion

     

        #region Public Properties

        public int ID
        {
            get { return id; }
            set { id = value; }
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
                if (string.IsNullOrEmpty(perName)) return "尚未填写";
                else
                    return perName;
            }
        }

        public int Sex
        {
            get { return sex; }
            set { sex = value; }
        }

        public string SexName
        {
            get { return NameProvider.GetSexName(sex); }
        }

        public string RName { get; set; }
        public string PhoneNum { get; set; }
        public int Age
        {
            get { return age; }
            set { age = value; }
        }

        public int LocationP
        {
            get { return location_P; }
            set { location_P = value; }
        }

        public int LocationC
        {
            get { return location_C; }
            set { location_C = value; }
        }

        public string LocationName
        {
            get { return NameProvider.GetProvinceName(location_P) + " " + NameProvider.GetCityName(location_C); }
        }

        public DateTime RegisterDate
        {
            get { return registerDate; }
            set { registerDate = value; }
        }

        public DateTime UpdateDate
        {
            get { return updateDate; }
            set { updateDate = value; }
        }

        public short CheckFlag
        {
            get { return checkFlag; }
            set { checkFlag = value; }
        }

        public string CheckFlagName
        {
            get { return NameOtherProvider.GetResumeCheckFlagName(checkFlag); }
        }

        public int RecommendId { get; set; }
        #endregion

        public int Index { get; set; }
    }
}
