using System;
using System.Collections.Generic;

namespace Entity.GoodjobInfo
{
    public partial class ArticleKeyword
    {
        public int Id { get; set; }
        public string Keyword { get; set; } = null!;
        public string HtmlCode { get; set; } = null!;
        public string Url { get; set; } = null!;
    }
}
