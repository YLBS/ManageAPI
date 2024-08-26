using System;
using System.Collections.Generic;

namespace Entity.GoodjobOther
{
    public partial class EmailAdministrator
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string UserPwd { get; set; } = null!;
        public DateTime RegisterDate { get; set; }
    }
}
