using System.Collections;
using System.Data;
using System.Reflection;
using AutoMapper;
using Dapper;
using Entity.Goodjob;
using IService.SalesDepartment;
using Microsoft.EntityFrameworkCore;
using Model.SalesDepartment;
using ServiceStack;

namespace Service.SalesDepartment
{

    public class PulicCompany : IPulicCompany
    {
        private readonly GoodjobContext _context;
        private readonly IMapper _mapper;
        public PulicCompany(GoodjobContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        #region 查询条件字段

        private const string FORMAT_MEMID = "Mem_Users.MemID";
        private const string FORMAT_MEMNAME = "Mem_Info.MemName";
        private const string FORMAT_ADDRESSP = "Mem_Info.Address_P";
        private const string FORMAT_ADDRESSC = "Mem_Info.Address_C";
        private const string FORMAT_ADDRESS = "Mem_Info.Address";
        private const string FORMAT_LASTLOGINDATE = "Mem_users.LastLoginDate";
        private const string FORMAT_SALERID = "Mem_Users.SalerUserID";
        private const string FORMAT_MEMBERTYPE = "Mem_Users.MemberClass";
        private const string FORMAT_BEGINVALIDDATE = "Mem_Users.BeginValidDate";
        private const string FORMAT_ENDVALIDDATE = "Mem_Users.EndValidDate";
        private const string FORMAT_UPDATEDATE = "Mem_Users.UpdateDate";
        private const string FORMAT_CHECKFLAG = "Mem_Users.CheckFlag";
        private const string FORMAT_CALLING = "Mem_Info.Calling";
        private const string FORMAT_EMAIL = "Mem_Info.Email";
        private const string FORMAT_TEL = "Mem_Info.ContactTel";
        private const string FORMAT_FAX = "Mem_Info.ContactFax";

        #endregion
        public async Task<IEnumerable<SalesCompanyListInfo>> GetSalesCompanyListInfo(SearchPulicCompany search)
        {
            var str = "";
            if (search != null)
                str = GetSqlString(search);
            var parm = new { where = str };
            var list1 = await _context.Database.GetDbConnection()
                .QueryAsync<SalesCompanyListInfo>("Sales_GetPublicCompany", parm, commandType: CommandType.StoredProcedure);
            return list1;

        }
        private string GetSqlString(SearchPulicCompany searchKey)
        {
            string sqlstring = string.Empty;
            PropertyInfo[] properties = searchKey.GetType().GetProperties();
            foreach (PropertyInfo property in properties)
            {
                var propertyValue = property.GetValue(searchKey);
                if (!Equals(propertyValue, property.PropertyType.GetDefaultValue()))
                {
                    var propertyName = property.Name;
                    if (string.IsNullOrEmpty(propertyName))
                    {
                        continue;
                    }
                    switch (propertyName)
                    {
                        case "MemID":
                            sqlstring += string.Format(propertyValue.ToString(), FORMAT_MEMID);
                            break;
                        case "MemName":
                            sqlstring += string.Format(propertyValue.ToString(), FORMAT_MEMNAME);
                            break;
                        case "AddressP":
                            sqlstring += string.Format(propertyValue.ToString(), FORMAT_ADDRESSP);
                            break;
                        case "AddressC":
                            sqlstring += string.Format(propertyValue.ToString(), FORMAT_ADDRESSC);
                            break;

                        case "AddressT":
                            sqlstring += string.Format(propertyValue.ToString(), "Mem_Info.Address_T");
                            break;
                        case "Address":
                            sqlstring += string.Format(propertyValue.ToString(), FORMAT_ADDRESS);
                            break;
                        case "LoginStart":
                            sqlstring += string.Format(propertyValue.ToString(), FORMAT_LASTLOGINDATE);
                            break;
                        case "LoginEnd":
                            sqlstring += string.Format(propertyValue.ToString(), FORMAT_LASTLOGINDATE);
                            break;
                        case "SalerID":
                            sqlstring += string.Format(propertyValue.ToString(), FORMAT_SALERID);
                            break;
                        case "MemberClass":
                            sqlstring += string.Format(propertyValue.ToString(), FORMAT_MEMBERTYPE);
                            break;
                        case "UpdateStart":
                            sqlstring += string.Format(propertyValue.ToString(), FORMAT_UPDATEDATE);
                            break;
                        case "UpdateEnd":
                            sqlstring += string.Format(propertyValue.ToString(), FORMAT_UPDATEDATE);
                            break;
                        case "StartServerStart":
                            sqlstring += string.Format(propertyValue.ToString(), FORMAT_BEGINVALIDDATE);
                            break;
                        case "StartServerEnd":
                            sqlstring += string.Format(propertyValue.ToString(), FORMAT_BEGINVALIDDATE);
                            break;
                        case "EndServerStart":
                            sqlstring += string.Format(propertyValue.ToString(), FORMAT_ENDVALIDDATE);
                            break;
                        case "EndServerEnd":
                            sqlstring += string.Format(propertyValue.ToString(), FORMAT_ENDVALIDDATE);
                            break;
                        case "Department":
                            sqlstring += string.Format(propertyValue.ToString(), FORMAT_SALERID);
                            break;
                        case "DepartSalerID":
                            sqlstring += string.Format(propertyValue.ToString(), FORMAT_SALERID);
                            break;
                        case "CheckFlag":
                            sqlstring += string.Format(propertyValue.ToString(), FORMAT_CHECKFLAG);
                            break;
                        case "Calling":
                            sqlstring += string.Format(propertyValue.ToString(), FORMAT_CALLING);
                            break;
                        case "Tel":
                            sqlstring += string.Format(propertyValue.ToString(), FORMAT_TEL);
                            break;
                        case "Fax":
                            sqlstring += string.Format(propertyValue.ToString(), FORMAT_FAX);
                            break;
                        case "Email":
                            sqlstring += string.Format(propertyValue.ToString(), FORMAT_EMAIL);
                            break;
                    }
                }
            }

            return sqlstring;
        }

        public async Task<IEnumerable<EffectCompanyAdInfo>> GetCompanyAd(int memId)
        {
            var parm = new { MemID = memId };
            var list1 = await _context.Database.GetDbConnection()
                .QueryAsync<EffectCompanyAdInfo>("Sales_Effect_CompanyAD", parm, commandType: CommandType.StoredProcedure);
           
            return list1;
        }

        public async Task<IEnumerable<EffectCompanyInfo>> GetCompanyInfo(int memId)
        {
            var list = await _context.MemInfos.Where(m=>m.MemId==memId).ToListAsync();
            var result = _mapper.Map<IEnumerable<EffectCompanyInfo>>(list);

            //原存储过程
            //var parm = new { MemID = memId };
            //var list1 = await _context.Database.GetDbConnection()
            //    .QueryFirstOrDefaultAsync<EffectCompanyInfo>("Sales_Effect_CompanyInfo", parm, commandType: CommandType.StoredProcedure);
            return result;
        }
    }
}
