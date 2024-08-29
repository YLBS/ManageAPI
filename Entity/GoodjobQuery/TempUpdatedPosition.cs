using System;
using System.Collections.Generic;

namespace Entity.GoodjobQuery
{
    public partial class TempUpdatedPosition
    {
        public int Id { get; set; }
        public int MemId { get; set; }
        public string MemName { get; set; } = null!;
        public int? PosId { get; set; }
        public string? PosName { get; set; }
        public string? JobLocationP { get; set; }
        public string? JobLocationC { get; set; }
        public int Calling { get; set; }
        public bool IsBuild { get; set; }
    }
}
