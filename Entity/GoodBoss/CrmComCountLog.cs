using System;
using System.Collections.Generic;

namespace Entity.GoodBoss
{
    public partial class CrmComCountLog
    {
        public int Id { get; set; }
        public int EplId { get; set; }
        public int ComCount { get; set; }
        public int Csid { get; set; }
        public DateTime LogTime { get; set; }
    }
}
