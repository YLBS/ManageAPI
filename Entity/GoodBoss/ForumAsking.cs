﻿using System;
using System.Collections.Generic;

namespace Entity.GoodBoss
{
    public partial class ForumAsking
    {
        public int Id { get; set; }
        public string Content { get; set; } = null!;
        public int EplId { get; set; }
        public string? EplName { get; set; }
        public DateTime? InTime { get; set; }
        public int? Type { get; set; }
        public string? Ipmac { get; set; }
    }
}
