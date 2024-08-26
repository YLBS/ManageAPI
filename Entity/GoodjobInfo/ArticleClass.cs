using System;
using System.Collections.Generic;

namespace Entity.GoodjobInfo
{
    public partial class ArticleClass
    {
        public int Id { get; set; }
        public string ClassName { get; set; } = null!;
        public int ParentId { get; set; }
        public int OrderId { get; set; }
        public string PageTitle { get; set; } = null!;
        public string PageKeyword { get; set; } = null!;
        public string PageDescription { get; set; } = null!;
        public byte PageType { get; set; }
        public int SiteId { get; set; }
        public byte SiteType { get; set; }
        public string FolderName { get; set; } = null!;
        public string TemplatePath { get; set; } = null!;
        public bool HasChild { get; set; }
        public bool Special { get; set; }
        public int ArticleType { get; set; }
        public bool IsHome { get; set; }
    }
}
