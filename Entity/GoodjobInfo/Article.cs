using System;
using System.Collections.Generic;

namespace Entity.GoodjobInfo
{
    public partial class Article
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string ArticleContent { get; set; } = null!;
        public string CoreContent { get; set; } = null!;
        public string Keyword { get; set; } = null!;
        public bool IsCommend { get; set; }
        public int VisitCount { get; set; }
        public int ClassId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = null!;
        public int EditorId { get; set; }
        public string EditorName { get; set; } = null!;
        public DateTime AddTime { get; set; }
        public DateTime EditTime { get; set; }
        public byte SiteType { get; set; }
        public string OldUrl { get; set; } = null!;
        public string RelateJobFunction { get; set; } = null!;
        public bool IsHot { get; set; }
        public bool IsHome { get; set; }
    }
}
