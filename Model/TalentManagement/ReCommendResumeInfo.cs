using Goodjob.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.TalentManagement
{
    /// <summary>
    /// 推荐简历管理
    /// </summary>
    public class ReCommendResumeInfo
    {

        public int MyUserID { set; get; }
        public string PerName { set; get; }
        public int Sex { set; get; }
        public int Location_P { set; get; }
        public int Location_C { set; get; }
        public DateTime RegisterDate { set; get; }
        public DateTime ComDate { set; get; }
        public DateTime UpdateDate { set; get; }
        
        public DateTime ReCommendDate
        {
            get {
                if (ComDate > UpdateDate)
                {
                    return ComDate;
                }
                return UpdateDate;
            }
        }

        public string Location=> NameProvider.GetProvinceName(Location_P) + " " + NameProvider.GetCityName(Location_C);
    }
}
