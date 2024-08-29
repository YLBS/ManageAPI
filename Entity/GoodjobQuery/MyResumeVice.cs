using System;
using System.Collections.Generic;

namespace Entity.GoodjobQuery
{
    public partial class MyResumeVice
    {
        public int Id { get; set; }
        public int EplId { get; set; }
        public string ResumeTitle { get; set; } = null!;
        public string Contact { get; set; } = null!;
        public string Appendix { get; set; } = null!;
        public DateTime AddTime { get; set; }
        public int ViceId { get; set; }
        public int ReplId { get; set; }
        public string EplName { get; set; } = null!;
        public string ReplName { get; set; } = null!;
        public DateTime? UpdataTime { get; set; }
        public string? ShareEplId { get; set; }
        public int RdepId { get; set; }
        public bool IsCommon { get; set; }
        public string Regions { get; set; } = null!;
    }
}
