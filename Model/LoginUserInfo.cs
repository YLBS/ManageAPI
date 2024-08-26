
namespace Model
{
    /// <summary>
    /// 登录后需要的用户信息
    /// </summary>
    public class LoginUserInfo
    {

        private int EplId;
        private string UserName;
        private string EplName;

        private int DptId;
        private int PosId;
        public LoginUserInfo() { }
        public LoginUserInfo(int EplId, string UserName, string EplName, int DptId, int PosId)
        {
            this.EplId = EplId;
            this.UserName = UserName;
            this.EplName = EplName;
            this.DptId = DptId;
            this.PosId = PosId;
        }
        
        public int UserId
        {
            get { return EplId; }
            set { EplId = value; }
        }

        public string Username
        {
            get { return UserName; }
            set { UserName = value; }
        }

        public string trueName
        {
            get { return EplName; }
            set { EplName = value; }
        }


        public int posId
        {
            get { return PosId; }
            set { PosId = value; }
        }
        public int dptId
        {
            get { return DptId; }
            set { DptId = value; }
        }
        
        public string[]? Roles { get; set; }

        public IEnumerable<ManagerDept> DeptName { get; set; }

        public IEnumerable<ManagerDeptUser> DeptUsers { get; set; }


        /// <summary>
        /// 部门
        /// </summary>
        public class ManagerDept
        {
            private int DeptId;
            private string DeptName;
            /// <summary>
            /// 构造方法隐藏真实的数据库字段
            /// </summary>

            public ManagerDept(int DeptId, string DeptName)
            {
                this.DeptId = DeptId;
                this.DeptName = DeptName;
            }
            public int Id
            {
                get { return DeptId; }
                set { DeptId = value; }
            }

            public string Name
            {
                get { return DeptName; }
                set { DeptName = value; }
            }
        }

        /// <summary>
        /// 部门职员
        /// </summary>
        public class ManagerDeptUser
        {
            /// <summary>
            /// 职员Id
            /// </summary>
            public int ClerkID
            {
                get; set;

            }
            public int DeptID
            {
                get; set;
            }
            public string ClerkName
            {
                get; set;
            }
        }

    }

}
