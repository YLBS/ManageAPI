﻿using System;
using System.Collections.Generic;

namespace Entity.GoodjobOther
{
    public partial class DataStatPosition
    {
        public int Id { get; set; }
        public int Ayear { get; set; }
        public int Amonth { get; set; }
        public int ItemId { get; set; }
        public int Agroup { get; set; }
        public int Amount { get; set; }
        public byte InFlag { get; set; }
        public DateTime InDate { get; set; }
    }
}
