using System;
using System.Collections.Generic;

namespace Entity.GoodjobQuery
{
    public partial class ArticleRelateJobFunction
    {
        public int Id { get; set; }
        public int NewsId { get; set; }
        public int RelateJobFunctionSmall { get; set; }
        public int RelateJobFunctionBig { get; set; }
    }
}
