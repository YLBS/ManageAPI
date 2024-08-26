using System;
using System.Collections.Generic;

namespace Entity.GoodjobInfo
{
    public partial class Image
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Introduce { get; set; } = null!;
        public string ImagePath { get; set; } = null!;
        public string SmallImagePath { get; set; } = null!;
        public int ClassId { get; set; }
        public byte SiteType { get; set; }
    }
}
