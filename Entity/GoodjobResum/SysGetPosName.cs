using System;
using System.Collections.Generic;

namespace Entity.GoodjobResum
{
    public partial class SysGetPosName
    {
        public int? MemId { get; set; }
        public string? MemName { get; set; }
        public int? PosId { get; set; }
        public string? PosName { get; set; }
        public string? JobFunction { get; set; }
        public int? MyUserId { get; set; }
        public DateTime? AddTime { get; set; }
    }
}
