using System;
using System.Collections.Generic;

namespace Entity.GoodjobQuery
{
    public partial class MemPosAiquery
    {
        public int PosId { get; set; }
        public int MemId { get; set; }
        public string MemName { get; set; } = null!;
        public string PosName { get; set; } = null!;
        public DateTime PostDate { get; set; }
        public byte Calling { get; set; }
        public int DeptId { get; set; }
        public int CandidateNum { get; set; }
        public byte Salary { get; set; }
        public byte PosType { get; set; }
        public int ReqDegreeId { get; set; }
        public byte ReqWorkYear { get; set; }
        public byte ReqSex { get; set; }
        public byte ReqAge1 { get; set; }
        public byte ReqAge2 { get; set; }
        public string PosDescription { get; set; } = null!;
        public string JobLocation { get; set; } = null!;
        /// <summary>
        /// 是否推荐企业
        /// </summary>
        public bool Iscommend { get; set; }
        /// <summary>
        /// 是否有专页
        /// </summary>
        public bool HasPage { get; set; }
        public bool Geyn { get; set; }
        public int? MemLocation { get; set; }
        public DateTime? MemRegisterDate { get; set; }
        public byte? Properity { get; set; }
        public string SeoAddress { get; set; } = null!;
        public bool Hires { get; set; }
        public string? Welfa { get; set; }
        public string? SalaryRange { get; set; }
        public double Lng { get; set; }
        public double Lat { get; set; }
        public string JobFunctionPart { get; set; } = null!;
        /// <summary>
        /// 转正机会  1 提供转正  2 不提供转正  3 面议
        /// </summary>
        public int RegularWorker { get; set; }
        public int Months { get; set; }
        /// <summary>
        /// 0 薪资类型 1 固定薪资 2范围薪资
        /// </summary>
        public int SalaryMode { get; set; }
        /// <summary>
        /// 固定薪资 用于存放 千元等 用户手动输入的金额
        /// </summary>
        public int PosSalary { get; set; }
        public int SalaryMin { get; set; }
        public int SalaryMax { get; set; }
        /// <summary>
        /// 薪资单位 1 元/每小时  2 元/每天  3 元/每周  4 元/每月  5 元/每次  6 元/每件
        /// </summary>
        public int SalaryUnit { get; set; }
        /// <summary>
        /// 0
        /// </summary>
        public int WeekRequired { get; set; }
        /// <summary>
        /// 0 俊才 1.前程 2.boss 3.58 4.智联 5拉钩
        /// </summary>
        public int RegisterBy { get; set; }
    }
}
