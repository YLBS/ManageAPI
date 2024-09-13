using Entity.Goodjob;
using IService.SalesDepartment;
using Microsoft.EntityFrameworkCore;
using Model.SalesDepartment;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;
using Model.Common;

namespace Service.SalesDepartment
{
    public class JobEffect : IJobEffect
    {
        private readonly GoodjobContext _context;

        public JobEffect(GoodjobContext context)
        {
            _context = context;
        }

        public async Task<IList<KeyValue>> CooperativeList(int userId, int state)
        {
            var param = new { UserID = userId, State = state };
            IList<KeyValue> k=new List<KeyValue>();
            List<int> Cooperativs = new List<int>();
            string sql = " select * from (select (select cc.MEM_ID from GoodBoss.dbo.CRM_Company as cc where cc.COM_ID= GoodBoss.dbo.client_collaborative.comID) as Mem_ID,collEplid,collEplname,eplID,eplName from GoodBoss.dbo.client_collaborative where [state] = @State and (collEplid = @UserID or eplID = @UserID) ) a where Mem_ID is  not null ";
            using (var reader = await _context.Database.GetDbConnection()
                       .ExecuteReaderAsync("Sales_Effect_PositionCount", param,
                           commandType: CommandType.StoredProcedure))
            {
                while (await reader.ReadAsync())
                {
                    int collEplid = Convert.ToInt32(reader["collEplid"]);
                    string collEplname = reader["collEplname"].ToString();
                    int eplID = Convert.ToInt32(reader["eplID"]);
                    string eplName = reader["eplName"].ToString();
                    int Mem_ID = reader["Mem_ID"] is DBNull ? 0 : Convert.ToInt32(reader["Mem_ID"]);
                    if (Mem_ID != 0)
                    {
                        if (collEplid == userId)
                        {
                            if (!Cooperativs.Contains(eplID))
                            {
                                Cooperativs.Add(eplID);
                                k.Add(new KeyValue(){Id = eplID,Name = eplName});
                            }
                        }

                        if (eplID == userId)
                        {
                            if (!Cooperativs.Contains(collEplid))
                            {
                                Cooperativs.Add(collEplid);
                                k.Add(new KeyValue() { Id = collEplid, Name = collEplname });
                            }
                        }
                    }
                }
            }

            return k;
        }

        public async Task<(IList<EffectPositionInfo> item,int count)> GetJobEffectCollection(string filter,string order)
        {
            var param = new { Filter = filter, Order = order };
            int count = 0;
            IList<EffectPositionInfo> positionCollection = new List<EffectPositionInfo>();
            using (var reader = await _context.Database.GetDbConnection().ExecuteReaderAsync("Sales_Effect_PositionCount", param, commandType: CommandType.StoredProcedure))
            {
                if (await reader.ReadAsync())
                {
                    count = reader.GetInt32(0);
                }

                if (count == 0)
                {
                    return (null, 0);
                }

                if (await reader.NextResultAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        EffectPositionInfo position = new EffectPositionInfo(reader.GetInt32(0), reader.GetInt32(1), reader.GetString(2), reader.GetString(3), (short)reader.GetByte(4), reader.GetDateTime(5), reader.GetDateTime(6), reader.GetInt32(7), reader.GetInt32(8), reader.GetInt32(9));
                        positionCollection.Add(position);
                    }
                }
            }

            return (positionCollection, count);
        }

        public async Task<(IList<EffectPositionInfo> item, int count)> NewGetJobEffectCollection(string filter, string order)
        {

            var param = new { Filter = filter, Order = order };
            int count = 0;
            IList<EffectPositionInfo> positionCollection = new List<EffectPositionInfo>();
            using (var reader = await _context.Database.GetDbConnection().ExecuteReaderAsync("Sales_Effect_PositionCount_New", param, commandType: CommandType.StoredProcedure))
            {
                if (await reader.ReadAsync())
                {
                    count = reader.GetInt32(0);
                }

                if (count == 0)
                {
                    return (null, 0);
                }

                if (await reader.NextResultAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        EffectPositionInfo position = new EffectPositionInfo(reader.GetInt32(0), reader.GetInt32(1), reader.GetString(2), reader.GetString(3), (short)reader.GetByte(4), reader.GetDateTime(5), reader.GetDateTime(6), reader.GetDateTime(7), reader.GetInt32(8), reader.GetInt32(9), reader.GetInt32(10), reader.GetInt32(11));
                        positionCollection.Add(position);
                    }
                }
            }
            return (positionCollection, count);
        }
    }
}
