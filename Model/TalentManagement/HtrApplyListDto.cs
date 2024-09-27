using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.TalentManagement
{
    public class HtrApplyListDto
    {
        //private string resume_Txt;
        //public HtrApplyListDto(){}
        //public HtrApplyListDto(string resume_Txt)
        //{
        //    this.resume_Txt = resume_Txt;
        //}
        public int Id
        {
            get;
            set;
        }
        public int? MyUserId
        {
            get;
            set;
        }
        public int? MemId
        {
            get;
            set;
        }
        public int? PosId
        {
            get;
            set;
        }
       
        public string? Resume_Txt
        {
            get;
            set;
        }
        public DateTime? ApplyTime
        {
            get;
            set;
        }
        public string? Phone
        {
            get;
            set;
        }
        public string? MemName
        {
            get;
            set;
        }
        public string? PosName
        {
            get;
            set;
        }
        public string? WordFile
        {
            get;
            set;
        }
        public int NewsMyuserId
        {
            get;
            set;
        }
        public string ResumeContext
        {
            get;
            set;
        } 
    }
}
