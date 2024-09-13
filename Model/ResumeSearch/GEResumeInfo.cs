using Goodjob.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ResumeSearch
{
    /// <summary>
    /// 普工简历
    /// </summary>
    public class GEResumeInfo
    {
        #region Member Variables

        private int m_id;
        private string m_username;
        private int m_sex;
        private int m_age;
        private string m_mobile;
        private string m_engwork;
        private string m_skills;
        private int m_memid;
        private int m_posid;
        private DateTime m_receivedate;
        private int m_applycount;
        private byte m_readflag;
        private bool m_isinterview;
        private string m_posName;
        private string m_memName;
        private int addressP; //地址省
        private int addressC; //地址市
        private int addressT; //地址县镇


        #endregion

        #region Constructors

        public GEResumeInfo()
        {
        }

        public GEResumeInfo(int id, string username, int sex, int age, string mobile, string engwork, string skills,
            int memid, int posid, DateTime receivedate, int applycount, byte readflag, bool isinterview)
        {
            this.m_id = id;
            this.m_username = username;
            this.m_sex = sex;
            this.m_age = age;
            this.m_mobile = mobile;
            this.m_engwork = engwork;
            this.m_skills = skills;
            this.m_memid = memid;
            this.m_posid = posid;
            this.m_receivedate = receivedate;
            this.m_applycount = applycount;
            this.m_readflag = readflag;
            this.m_isinterview = isinterview;
        }

        /// <summary>
        /// 普工简历搜索用
        /// </summary>
        /// <param name="id"></param>
        /// <param name="username"></param>
        /// <param name="sex"></param>
        /// <param name="age"></param>
        /// <param name="mobile"></param>
        /// <param name="engwork"></param>
        /// <param name="skills"></param>
        /// <param name="memid"></param>
        /// <param name="posid"></param>
        /// <param name="receivedate"></param>
        /// <param name="applycount"></param>
        /// <param name="readflag"></param>
        /// <param name="isinterview"></param>
        /// <param name="posname"></param>
        /// <param name="memname"></param>
        public GEResumeInfo(int id, string username, int sex, int age, string mobile, string engwork, string skills,
            int memid, int posid, DateTime receivedate, int applycount, byte readflag, bool isinterview, string posname,
            string memname)
        {
            this.m_id = id;
            this.m_username = username;
            this.m_sex = sex;
            this.m_age = age;
            this.m_mobile = mobile;
            this.m_engwork = engwork;
            this.m_skills = skills;
            this.m_memid = memid;
            this.m_posid = posid;
            this.m_receivedate = receivedate;
            this.m_applycount = applycount;
            this.m_readflag = readflag;
            this.m_isinterview = isinterview;
            this.m_posName = posname;
            this.m_memName = memname;
        }

        /// <summary>
        /// 普工简历详情
        /// </summary>
        /// <param name="id"></param>
        /// <param name="username"></param>
        /// <param name="sex"></param>
        /// <param name="age"></param>
        /// <param name="mobile"></param>
        /// <param name="engwork"></param>
        /// <param name="skills"></param>
        /// <param name="memid"></param>
        /// <param name="posid"></param>
        /// <param name="receivedate"></param>
        /// <param name="applycount"></param>
        /// <param name="readflag"></param>
        /// <param name="isinterview"></param>
        /// <param name="posname"></param>
        /// <param name="memname"></param>
        /// <param name="addressp"></param>
        /// <param name="addressc"></param>
        /// <param name="addresst"></param>
        public GEResumeInfo(int id, string username, int sex, int age, string mobile, string engwork, string skills,
            int memid, int posid, DateTime receivedate, int applycount, byte readflag, bool isinterview, string posname,
            string memname, int addressp, int addressc, int addresst)
        {
            this.m_id = id;
            this.m_username = username;
            this.m_sex = sex;
            this.m_age = age;
            this.m_mobile = mobile;
            this.m_engwork = engwork;
            this.m_skills = skills;
            this.m_memid = memid;
            this.m_posid = posid;
            this.m_receivedate = receivedate;
            this.m_applycount = applycount;
            this.m_readflag = readflag;
            this.m_isinterview = isinterview;
            this.m_posName = posname;
            this.m_memName = memname;
            this.addressP = addressp;
            this.addressC = addressc;
            this.addressT = addresst;
        }

        #endregion

        #region Public Properties

        public int ID
        {
            set { this.m_id = value; }
            get { return m_id; }
        }

        public string UserName
        {
            set { this.m_username = value; }
            get { return m_username; }
        }

        public int Sex
        {
            set { this.m_sex = value; }
            get { return m_sex; }
        }

        public int Age
        {
            set { this.m_age = value; }
            get { return m_age; }
        }

        public string Mobile
        {
            set { this.m_mobile = value; }
            get { return m_mobile; }
        }

        public string EngWork
        {
            set { this.m_engwork = value; }
            get { return m_engwork; }
        }

        public string Skills
        {
            set { this.m_skills = value; }
            get { return m_skills; }
        }

        public int MemID
        {
            set { this.m_memid = value; }
            get { return m_memid; }
        }

        public int PosID
        {
            set { this.m_posid = value; }
            get { return m_posid; }
        }

        public DateTime ReceiveDate
        {
            set { this.m_receivedate = value; }
            get { return m_receivedate; }
        }

        public int ApplyCount
        {
            set { this.m_applycount = value; }
            get { return m_applycount; }
        }

        public byte ReadFlag
        {
            set { this.m_readflag = value; }
            get { return m_readflag; }
        }

        public bool IsInterview
        {
            set { this.m_isinterview = value; }
            get { return m_isinterview; }
        }

        public string PosName
        {
            set { this.m_posName = value; }
            get { return this.m_posName; }
        }

        public string MemName
        {
            set { this.m_memName = value; }
            get { return this.m_memName; }
        }


        /// <summary>
        /// 地址 省ID
        /// </summary>
        public int AddressP
        {
            get { return addressP; }
            set { addressP = value; }
        }

        /// <summary>
        /// 地址市id
        /// </summary>
        public int AddressC
        {
            get { return addressC; }
            set { addressC = value; }
        }

        public string AddressPandCName
        {
            get { return NameProvider.GetProvinceName(AddressP) + NameProvider.GetCityName(AddressC); }
        }

        public int AddressT
        {

            get { return addressT; }
            set { addressT = value; }

        }

        #endregion
    }
}
