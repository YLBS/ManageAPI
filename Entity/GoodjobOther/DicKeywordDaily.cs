using System;
using System.Collections.Generic;

namespace Entity.GoodjobOther
{
    public partial class DicKeywordDaily
    {
        public int Id { get; set; }
        public string Keyword { get; set; } = null!;
        public DateTime AddDate { get; set; }
    }
}
