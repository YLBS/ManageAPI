using System;
using System.Collections.Generic;

namespace Entity.GoodjobOther
{
    public partial class ServiceNumber
    {
        public int Id { get; set; }
        public int MemId { get; set; }
        public int SalerId { get; set; }
        public DateTime? Datetime { get; set; }
    }
}
