﻿using System;
using System.Collections.Generic;

namespace Entity.GoodBoss
{
    public partial class ViewErmEmployee
    {
        public int EplId { get; set; }
        public string UserName { get; set; } = null!;
        public string UserPwd { get; set; } = null!;
        public string EplName { get; set; } = null!;
        public string? Sex { get; set; }
        public string? Marry { get; set; }
        public string? Picture { get; set; }
        public DateTime Birthday { get; set; }
        public string Email { get; set; } = null!;
        public string Mobile { get; set; } = null!;
        public string PhoneZ { get; set; } = null!;
        public string PhoneN { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string PhoneE { get; set; } = null!;
        public string IdCard { get; set; } = null!;
        public int DgrId { get; set; }
        public string SchoolName { get; set; } = null!;
        public string Speciality { get; set; } = null!;
        public int? Age { get; set; }
        public string Diploma { get; set; } = null!;
        public string Headship { get; set; } = null!;
        public string Protocol { get; set; } = null!;
        public string Tongue { get; set; } = null!;
        public string Nation { get; set; } = null!;
        public string? ProvId { get; set; }
        public int CityId { get; set; }
        public string Area { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string PostCode { get; set; } = null!;
        public string LnkMan { get; set; } = null!;
        public string LnkTel { get; set; } = null!;
        public int PosId { get; set; }
        public int DptId { get; set; }
        public int EsId { get; set; }
        public int Enable { get; set; }
        public bool LimitIp { get; set; }
        public string IpAddress { get; set; } = null!;
        public DateTime? LeaveDate { get; set; }
        public DateTime EnterDate { get; set; }
        public DateTime? PosiDate { get; set; }
        public string? Bqq { get; set; }
        public string Interest { get; set; } = null!;
        public string Memo { get; set; } = null!;
        public DateTime RegisterDate { get; set; }
        public string? Education { get; set; }
        public string? DptName { get; set; }
        public int? Expr1 { get; set; }
        public int Expr2 { get; set; }
        public string? EsName { get; set; }
        public int EnterprisePoints { get; set; }
    }
}
