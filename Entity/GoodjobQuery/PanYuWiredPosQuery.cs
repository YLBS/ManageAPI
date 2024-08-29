using System;
using System.Collections.Generic;

namespace Entity.GoodjobQuery
{
    public partial class PanYuWiredPosQuery
    {
        public int PosId { get; set; }
        public int MemId { get; set; }
        public string MemName { get; set; } = null!;
        public string PosName { get; set; } = null!;
        public DateTime PostDate { get; set; }
        public string Welfa { get; set; } = null!;
        public string SeoAddress { get; set; } = null!;
        public string PosDescription { get; set; } = null!;
        public string JobLocation { get; set; } = null!;
        public string SalaryRange { get; set; } = null!;
        public int ReqDegreeId { get; set; }
        public byte Salary { get; set; }
        public byte ReqWorkYear { get; set; }
        public bool Hires { get; set; }
        public bool IsCommend { get; set; }
        public int HitCounter { get; set; }
        public bool IsDell { get; set; }
    }
}
