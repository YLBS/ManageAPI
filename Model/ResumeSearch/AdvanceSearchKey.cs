using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ResumeSearch
{
    /// <summary>
    /// 简历高级搜索参数
    /// </summary>
    public class AdvanceSearchKey
    {
        /// <summary>
        /// 期望工作地区
        /// </summary>
        public string? HopeArea { get; set; } = null!;
        /// <summary>
        /// 期望工作,只能选5个
        /// </summary>
        public string? HopeWork { get; set; } = null!;
        /// <summary>
        /// 最近职位
        /// </summary>
        public string? LastPos { get; set; } = null!;

        /// <summary>
        /// 语言
        /// </summary>
        public int Language { get; set; } 
        /// <summary>
        /// 语言水平
        /// </summary>
        public int LanguageLevel { get; set; }
        /// <summary>
        /// 工作经验
        /// </summary>
        public int WorkYear1 { get; set; }
        /// <summary>
        /// 工作经验
        /// </summary>
        public int WorkYear2 { get; set; }
        /// <summary>
        /// 性别，0 不限，1 男，2 女
        /// </summary>
        public int Sex { get; set; }
        /// <summary>
        /// 目前所在地区
        /// </summary>
        public int LocationP { get; set; } = 0;
        /// <summary>
        /// 目前所在地区
        /// </summary>
        public int LocationC { get; set; } = 0;
        /// <summary>
        /// 目前所在地区
        /// </summary>
        public int LocationT { get; set; } = 0;
        /// <summary>
        /// 年龄
        /// </summary>
        public int Age1 { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        public int Age2 { get; set; }
        /// <summary>
        /// 户籍所在地 省
        /// </summary>
        public int HometownP { get; set; } = 0;
        /// <summary>
        /// 户籍所在地 市
        /// </summary>
        public int HometownC { get; set; } = 0;
        /// <summary>
        /// 学历要求
        /// </summary>

        public int Degree1 { get; set; }
        /// <summary>
        /// 学历要求
        /// </summary>
        public int Degree2 { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public int UpdateDate { get; set; }

        /// <summary>
        /// 毕业学校
        /// </summary>
        public string? School { get; set; }
        /// <summary>
        /// 关键字或简历编号
        /// </summary>
        public string? Keyword { get; set; } = null!;
        /// <summary>
        /// 所学专业
        /// </summary>
        public string? Special { get; set; } = null!;

        /// <summary>
        /// 婚姻状态
        /// </summary>
        public int MaritalStatus { get; set; }

        /// <summary>
        /// 是否有照片
        /// </summary>
        public int HavePhoto { get; set; }
        /// <summary>
        /// 是否有英文简历
        /// </summary>
        public int HaveEnglish { get;set; }
        /// <summary>
        /// 简历状态
        /// </summary>
        public string? ResumeStatus { get; set; }
        /// <summary>
        /// 排序，1 按简历更新排序，2 按简历注册时间排序
        /// </summary>
        public int OrderBy { get; set; }
    }
}
