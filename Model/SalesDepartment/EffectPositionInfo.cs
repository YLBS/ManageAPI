using Goodjob.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.SalesDepartment
{
    public class EffectPositionInfo
    {
        #region Member Variables
        private int posID;
        private string posName;
        private string memName;
        private short posState;
        private short calling;
        //private string deptName;
        private int candidatesNum;
        private int reqDegreeID;
        private short reqWorkYear;
        //private short reqSex;
        private DateTime registerDate;
        private DateTime postDate;
        private int hitCounter;
        private string jobLocation;
        private string jobFunction;

        private int totalApply;
        private int totalInvite;

        private int totalUnViewd;
        private int totalDeleted;

        private int todayReceived;
        private int totalCommend;

        private IDictionary<int, int> degreeTotal;
        private IDictionary<int, int> workyearTotal;

        private int memID;

        private DateTime endValidDate;

        private DateTime commendDate;

        private int openTimes;
        #endregion

        #region Constructors
        public EffectPositionInfo() { }

        /// <summary>
        /// 职位数据分析,职位列表用
        /// </summary>
        public EffectPositionInfo(int posID, string posName, short posState, DateTime registerDate, DateTime postDate, int reqDegreeID, short reqWorkYear, short calling, string jobLocation, string jobFunction, int candidatesNum, int hitCounter, int totalApply, int totalInvite, int totalUnViewd, int totalDeleted, int todayReceived, int totalCommend,int salary,int orderState) //20240820补充 salary、orderState
        {
            this.posID = posID;
            this.posName = posName;
            this.posState = posState;
            this.registerDate = registerDate;
            this.postDate = postDate;
            this.reqDegreeID = reqDegreeID;
            this.reqWorkYear = reqWorkYear;
            this.calling = calling;
            this.jobLocation = jobLocation;
            this.jobFunction = jobFunction;
            //
            this.candidatesNum = candidatesNum;
            this.hitCounter = hitCounter;
            this.totalApply = totalApply;
            this.totalInvite = totalInvite;

            this.totalUnViewd = totalUnViewd;
            this.totalDeleted = totalDeleted;

            this.todayReceived = todayReceived;
            this.totalCommend = totalCommend;
        }

        /// <summary>
        /// 职位发布效果
        /// </summary>        
        public EffectPositionInfo(int posID, string posName, short posState, int candidatesNum, int hitCounter, int totalApply, int totalInvite, string memName)
        {
            this.posID = posID;
            this.posName = posName;
            this.posState = posState;
            this.candidatesNum = candidatesNum;
            this.hitCounter = hitCounter;
            this.totalApply = totalApply;
            this.totalInvite = totalInvite;
            this.memName = memName;
        }

        /// <summary>
        /// 单个职位应聘简历分析
        /// </summary>
        public EffectPositionInfo(int posID, string posName, short posState, DateTime registerDate, DateTime postDate, string memName, int memID)
        {
            this.posID = posID;
            this.posName = posName;
            this.posState = posState;
            this.registerDate = registerDate;
            this.postDate = postDate;
            this.memName = memName;
            this.memID = memID;
        }

        /// <summary>
        /// 单个职位类别的职位
        /// </summary>
        public EffectPositionInfo(int posID, int memID, string posName, string memName, DateTime postDate, string jobLocation, string jobFunction)
        {
            this.posID = posID;
            this.memID = memID;
            this.posName = posName;
            this.memName = memName;
            this.postDate = postDate;
            this.jobLocation = jobLocation;
            this.jobFunction = jobFunction;
        }

        /// <summary>
        /// 职位发布效果
        /// </summary>
        public EffectPositionInfo(int posID, int memID, string posName, string memName, short posState, DateTime endValidDate, DateTime postDate, int candidatesNum, int hitCounter, int totalApply)
        {
            this.posID = posID;
            this.memID = memID;
            this.posName = posName;
            this.memName = memName;
            this.posState = posState;
            this.endValidDate = endValidDate;
            this.postDate = postDate;
            this.candidatesNum = candidatesNum;
            this.hitCounter = hitCounter;
            this.totalApply = totalApply;
        }
        /// <summary>
        /// 职位发布效果 2
        /// </summary>
        public EffectPositionInfo(int posID, int memID, string posName, string memName, short posState, DateTime endValidDate, DateTime registerDate, DateTime postDate, int candidatesNum, int hitCounter, int totalApply, int openTimes)
        {
            this.posID = posID;
            this.memID = memID;
            this.posName = posName;
            this.memName = memName;
            this.posState = posState;
            this.endValidDate = endValidDate;
            this.registerDate = registerDate;
            this.postDate = postDate;
            this.candidatesNum = candidatesNum;
            this.hitCounter = hitCounter;
            this.totalApply = totalApply;
            this.openTimes = openTimes;
        }

        /// <summary>
        /// 获取客服推荐职位
        /// </summary>
        /// <param name="posID"></param>
        /// <param name="memID"></param>
        /// <param name="posName"></param>
        /// <param name="memName"></param>
        /// <param name="posState"></param>
        /// <param name="commendDate"></param>
        public EffectPositionInfo(int posID, int memID, string posName, string memName, short posState, DateTime commendDate)
        {
            this.posID = posID;
            this.memID = memID;
            this.posName = posName;
            this.memName = memName;
            this.posState = posState;
            this.commendDate = commendDate;
        }
        #endregion
        

        #region Public Properties

        public string CommendDateName
        {
            get { return commendDate.ToString("g"); }
        }

        public int PosID
        {
            get { return posID; }
        }

        public int MemID
        {
            get { return memID; }
        }

        public string PosName
        {
            get { return posName; }
        }

        public string MemName
        {
            get { return memName; }
        }

        public string PosStateName
        {
            get { return NameOtherProvider.GetPosStateName(posState); }
        }

        public string RegisterDate
        {
            get { return registerDate.ToString("d"); }
        }

        public string PostDate
        {
            get { return postDate.ToString("g"); }
        }

        public short Calling
        {
            get { return calling; }
        }

        public string CallingName
        {
            get { return NameProvider.GetIndustryName(calling); }
        }

        public string CandidatesNumName
        {
            get
            {
                if (candidatesNum == 0) return "若干";
                return candidatesNum.ToString() + " 人";
            }
        }

        public int HitCounter
        {
            get { return hitCounter; }
        }

        public int ReqDegreeID
        {
            get { return reqDegreeID; }
        }

        public string ReqDegreeName
        {
            get { return NameProvider.GetDegreeName(reqDegreeID); }
        }

        public short ReqWorkYear
        {
            get { return reqWorkYear; }
        }

        public string ReqWorkYearName
        {
            get { return NameProvider.GetReqWorkYearName(reqWorkYear); }
        }

        public string JobLocation
        {
            get { return jobLocation; }
        }

        public string JobFunction
        {
            get { return jobFunction; }
        }

        public int TotalApply
        {
            get { return totalApply; }
        }

        public int TotalInvite
        {
            get { return totalInvite; }
        }

        public IDictionary<int, int> DegreeTotal
        {
            get { return degreeTotal; }
            set { degreeTotal = value; }
        }

        public IDictionary<int, int> WorkyearTotal
        {
            get { return workyearTotal; }
            set { workyearTotal = value; }
        }

        public int TotalUnViewd
        {
            get { return totalUnViewd; }
        }

        public int TotalDeleted
        {
            get { return totalDeleted; }
        }

        public int TotalCommend
        {
            get { return totalCommend; }
        }

        public int TodayReceived
        {
            get { return todayReceived; }
        }

        public string EndValidDate
        {
            get { return endValidDate.ToString("d"); }
        }

        public int OpenTimes
        {
            get { return this.openTimes; }
        }
        #endregion
    }
}
