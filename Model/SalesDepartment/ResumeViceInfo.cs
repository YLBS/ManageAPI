namespace Model.SalesDepartment
{
    public class ResumeViceInfo
    {
        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        private int epl_id;

        public int Epl_id
        {
            get { return epl_id; }
            set { epl_id = value; }
        }
        private string resumeTitle;

        public string ResumeTitle
        {
            get { return resumeTitle; }
            set { resumeTitle = value; }
        }
        private string contact;

        public string Contact
        {
            get { return contact; }
            set { contact = value; }
        }
        private string appendix;

        public string Appendix
        {
            get { return appendix; }
            set { appendix = value; }
        }
        private DateTime addTime;

        public DateTime AddTime
        {
            get { return addTime; }
            set { addTime = value; }
        }

        public string epl_Name;

        public string EPL_Name
        {
            get { return epl_Name; }
            set { epl_Name = value; }
        }

        public string repl_Name;

        public string REPL_Name
        {
            get { return repl_Name; }
            set { repl_Name = value; }
        }

        private int repl_id;

        public int REpl_id
        {
            get { return repl_id; }
            set { repl_id = value; }
        }

        private int viceId;

        public int ViceId
        {
            get { return viceId; }
            set { viceId = value; }
        }
        /// <summary>
        /// 推荐次数
        /// </summary>
        private int rCount;

        public int RCount
        {
            get { return rCount; }
            set { rCount = value; }
        }


        private int rdepId;

        public int RDepId
        {
            get { return rdepId; }
            set { rdepId = value; }
        }

        private bool isCommon;

        public bool IsCommon
        {
            get { return isCommon; }
            set { isCommon = value; }
        }
        public ResumeViceInfo() { }

        public ResumeViceInfo(int id, int epl_id, string resumeTitle, string contact, string appendix, DateTime addTime)
        {
            this.id = id;
            this.epl_id = epl_id;
            this.resumeTitle = resumeTitle;
            this.contact = contact;
            this.appendix = appendix;
            this.addTime = addTime;
        }
        /// <summary>
        /// 我的简历库 用到
        /// </summary>
        public ResumeViceInfo(int id, int epl_id, string resumeTitle, string contact, string appendix, DateTime addTime, int viceId, int repl_id, string epl_Name, string repl_Name, int rCount, int rdepId)
        {
            this.id = id;//0
            this.epl_id = epl_id;//1
            this.resumeTitle = resumeTitle;//2
            this.contact = contact;//3
            this.appendix = appendix;//4
            this.addTime = addTime;//5
            this.viceId = viceId;//6
            this.repl_id = repl_id;//7
            this.epl_Name = epl_Name;//8
            this.repl_Name = repl_Name;//9
            this.rCount = rCount;
            this.rdepId = rdepId;
        }

        public string Regions { get; set; }

        public string RUrl { get; set; }

        public int MyUserId { get; set; }
        public string PerName { get; set; }
        public int RegisterFrom { get; set; }
    }
}
