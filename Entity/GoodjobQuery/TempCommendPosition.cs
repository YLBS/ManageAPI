using System;
using System.Collections.Generic;

namespace Entity.GoodjobQuery
{
    public partial class TempCommendPosition
    {
        public int Id { get; set; }
        public int MemId { get; set; }
        public string MemName { get; set; } = null!;
        public int PosId { get; set; }
        public string PosName { get; set; } = null!;
        public string? JobLocationP { get; set; }
        public string? JobLocationC { get; set; }
        public bool IsBuild { get; set; }
    }
}
