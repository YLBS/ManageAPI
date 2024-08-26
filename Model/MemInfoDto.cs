using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class MemInfoDto
    {
        public int MemId { get; set; }
        public string MemName { get; set; } = null!;
        public string LicenceNumber { get; set; } = null!;
        public byte Calling { get; set; }
        public byte Properity { get; set; }
        public DateTime FoundDate { get; set; }
        public byte RegisterFund { get; set; }
        public byte EmployeeNumber { get; set; }
        public string CompanyIntroduction { get; set; } = null!;
        public string ContactPerson { get; set; } = null!;
        public int RegisterBy { get; set; }
        public int MaxHiresNum { get; set; }
        public int SumHires { get; set; }
        public string PhoneNum { get; set; } = null!;
        public int PosNum { get; set; }
        public bool Isvideo { get; set; }
        public DateTime RequestVideoDate { get; set; }
        public bool? IsSearchResume { get; set; }

    }
}
