using IService.TalentManagement;
using Model.TalentManagement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Entity.Goodjob;
using Microsoft.EntityFrameworkCore;

namespace Service.TalentManagement
{
    public class MessageList : IMessageList
    {
        private readonly GoodjobContext _context;

        public MessageList(GoodjobContext context)
        {
            _context = context;
        }

       

        public async Task<IEnumerable<PersonMessageInfo>> GetSalerMessCollection(string filter)
        {
            var parameters = new { where = filter};
            var result = await _context.Database.GetDbConnection().QueryAsync<PersonMessageInfo>("Info_My_GetMessageList_Clerk", parameters, commandType: CommandType.StoredProcedure);
            return result;
        }
        public async Task<bool> DeleteSalerMessage(int infoId)
        {
            var s=await _context.PubInformationMies.Where(p=>p.InfoId==infoId).FirstOrDefaultAsync();
            if (s!=null)
                s.IsDelete=true;
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
