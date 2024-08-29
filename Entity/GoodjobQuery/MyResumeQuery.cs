using System;
using System.Collections.Generic;

namespace Entity.GoodjobQuery
{
    public partial class MyResumeQuery
    {
        public int MyUserId { get; set; }
        public string PerName { get; set; } = null!;
        public byte Sex { get; set; }
        public DateTime Birthday { get; set; }
        public int HometownP { get; set; }
        public int HometownC { get; set; }
        public int LocationP { get; set; }
        public int LocationC { get; set; }
        public int DegreeId { get; set; }
        public string SchoolName { get; set; } = null!;
        public string Speciality { get; set; } = null!;
        public int WorkedYear { get; set; }
        public string LastPosName { get; set; } = null!;
        public byte EngResumeFlag { get; set; }
        public byte ContactStatus { get; set; }
        public byte PhotoFlag { get; set; }
        public DateTime? UpdateDate { get; set; }
        public byte? ResumeStatus { get; set; }
        public int LocationT { get; set; }
        /// <summary>
        /// 简历所属 0为俊才 1为大岗 2为南沙 4为黄阁
        /// </summary>
        public int BelongType { get; set; }
        public string Grade { get; set; } = null!;
        public string Professional { get; set; } = null!;
    }
}
