using IService.SalesDepartment;
using Model.SalesDepartment;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Entity.Goodjob;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Dapper;

namespace Service.SalesDepartment
{
    public class MessageLis : IMessageLis
    {
        private readonly GoodjobContext _context;
        
        public MessageLis(GoodjobContext context)
        {
            this._context = context;
        }

        private const string PUB_SALERID = "SalerID";
        private const string PUB_MEMID = "MemID";
        private const string PUB_MEMNAME = "MemName";
        private const string PUB_SUBJECT = "Subject";
        private const string PUB_READ = "Read";
        private const string PUB_SUEDFROM = "IssuedFrom";
        public async Task<IEnumerable<SalesMessageInfo>> GetData(SearchMessageLis searchMessageLis)
        {
            var str = "";
            if (searchMessageLis != null)
                str = GetSqlString(searchMessageLis);
            var parm = new { where = str };
            var list1 = await _context.Database.GetDbConnection()
                .QueryAsync<SalesMessageInfo>("Info_GetMessageList_Saler", parm, commandType: CommandType.StoredProcedure);
            return list1;
        }

        
        private string GetSqlString(SearchMessageLis searchKey)
        {
            string sqlstr = string.Empty;
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
                        case "SalerID":
                            sqlstr += string.Format(propertyValue.ToString(), PUB_SALERID);
                            break;
                        case "MemID":
                            sqlstr += string.Format(propertyValue.ToString(), PUB_MEMID);
                            break;
                        case "MemName":
                            sqlstr += string.Format(propertyValue.ToString(), PUB_MEMID, PUB_MEMNAME);
                            break;
                        case "Subject":
                            sqlstr += string.Format(propertyValue.ToString(), PUB_SUBJECT);
                            break;
                        case "Read":
                            sqlstr += string.Format(propertyValue.ToString(), PUB_READ);
                            break;
                        case "Sender":
                            sqlstr += string.Format(propertyValue.ToString(), PUB_SUEDFROM);
                            break;
                    }
                }
            }

            return sqlstr;
        }

        public async Task<bool> DeleteDeleteMessage(int infoId, int userId)
        {
            //只能删除自己的，企业发布的不能删除
            var list = await _context.PubInformations
                .Where(p => p.InfoId == infoId && p.SalerId == userId && p.IssuedFrom != 1).FirstOrDefaultAsync();
            if (list == null)
            {
                return false;
            }
            list.IsDelete= true;
            return await _context.SaveChangesAsync()>0;
        }
    }
}
