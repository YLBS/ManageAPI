using System;
using System.Collections.Generic;

namespace Entity.GoodjobOther
{
    public partial class DicProductType
    {
        public int Id { get; set; }
        public string Pname { get; set; } = null!;
        public string? Ename { get; set; }
        public int OrderId { get; set; }
    }
}
