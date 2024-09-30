using IService.TalentManagement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Entity.Goodjob;
using Microsoft.EntityFrameworkCore;

namespace Service.TalentManagement
{
    public class ResumeComplete : IResumeComplete
    {
        private readonly GoodjobContext _context;
        public ResumeComplete(GoodjobContext context)
        {
            _context = context;
        }

        public async Task<bool> InsertCompleteQd(int userId, int myUserId, string describe, int qdType)
        {
            var parameters = new { @myuserid = myUserId, @Describe = describe, @QdType= qdType, @userId= userId };
            var result = await _context.Database.GetDbConnection().ExecuteAsync("Mng_Insert_CompleteQd", parameters, commandType: CommandType.StoredProcedure);
            return result > 0;
        }
    }
}
