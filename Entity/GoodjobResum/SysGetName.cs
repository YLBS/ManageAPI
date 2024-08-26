using System;
using System.Collections.Generic;

namespace Entity.GoodjobResum
{
    public partial class SysGetName
    {
        public int MyUserId { get; set; }
        public string? MyName { get; set; }
        public int? Sex { get; set; }
        public string? UserName { get; set; }
        public string? PassWord { get; set; }
        public string? Email { get; set; }
        public DateTime? AddTime { get; set; }
    }
}
