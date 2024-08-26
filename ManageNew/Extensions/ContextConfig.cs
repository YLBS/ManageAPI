using Entity.GoodBoss;
using Entity.Goodjob;
using Entity.GoodjobInfo;
using Entity.GoodjobOther;
using Entity.GoodjobResum;
using Microsoft.EntityFrameworkCore;

namespace ManageNew.Extensions;
/// <summary>
/// 连接数据库配置
/// </summary>
public static class ContextConfig
{
    /// <summary>
    /// 添加连接数据库字符串
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    public static void AddDbContexts(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddDbContext<GoodjobContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("GoodJobConnection")));
        
        services.AddDbContext<GoodBossContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("GoodBossConnection")));

        services.AddDbContext<Goodjob_infoContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("GoodJobInfoConnection")));

        services.AddDbContext<Goodjob_OtherContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("GoodJobOtherConnection")));
        string s = configuration.GetConnectionString("GoodBossConnection");
        services.AddDbContext<Goodjob_resumeContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("GoodJobReSumConnection")));
    }
}
