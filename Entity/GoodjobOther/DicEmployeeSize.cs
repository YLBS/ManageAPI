using System;
using System.Collections.Generic;

namespace Entity.GoodjobOther
{
    public partial class DicEmployeeSize
    {
        public int Id { get; set; }
        public string Pname { get; set; } = null!;
        public string Ename { get; set; } = null!;
        public int OrderId { get; set; }
    }
}
