using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class OutJiuYeStationDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string AffiliatedUnit { get; set; } = null!;
        public string Introduce { get; set; } = null!;
        public string Eaddress { get; set; } = null!;
        public int EaddressP { get; set; }
        public int EaddressC { get; set; }
        public int EaddressD { get; set; }
        public int EaddressT { get; set; }
        public string? TopImage { get; set; }
        public string? ListImage { get; set; }
        public string? EnvironmentImage { get; set; }

        public int MemCount { get; set; }

        public int PosCount { get; set; }

        public int PeopleCount { get; set; }
        public int BelongType { get; set; }

    }
}
