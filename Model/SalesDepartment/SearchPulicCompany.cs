using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.SalesDepartment
{
    /// <summary>
    /// 搜索 传的是字典。。。  dicty.Add("MemID", " and {0}="+WebHelper.GetQueryString("MemID"));
    /// </summary>
    public class SearchPulicCompany
    {
        public string? MemID { get; set; }
        public string? Email { get; set; }
        public string? Fax { get; set; }
        public string? Tel { get; set; }
        public string? MemName { get; set; }
        public string? AddressP { get; set; }
        public string? AddressC { get; set; }
        public string? AddressT { get; set; }
        public string? LoginStart { get; set; }
        public string? LoginEnd { get; set; }
        public string? Calling { get; set; }
        public string? MemberClass { get; set; }
        public string? Address { get; set; }
    }
}
