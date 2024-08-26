using Common.Cache;
using Common.ConfigOptions;
using Common.Tool;
using ManageNew.Authentication.JWT;
using ManageNew.CacheManageTool;
using ManageNew.Controllers.Common;
using ManageNew.ExceptionFilter;
using ManageNew.Extensions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

#region 全局异常过滤器

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ExceptionFilter>();
});
#endregion

//builder.Services.AddScoped<ITest, Test>();




builder.Services.AddScoped<ITokenService, TokenService>();

//依赖注册接口和实现类
builder.Services.AddDi();
builder.Services.AddScoped<CheckPermission>();
//添加缓存
builder.Services.AddResponseCaching(); //只适用于get请求
builder.Services.AddMemoryCache();

builder.Services.AddControllers();

#region JWT验证授权
//初始化JwtAuthOptions
builder.Services.Configure<JwtAuthOptions>(builder.Configuration.GetSection("JwtAuth"));

builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddJwtAuthorization();
#endregion

builder.Services.AddEndpointsApiExplorer();

//实体映射配置
var mapper = AutoMapperConfig.ConfigureAutoMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddSwaggerSetup();
builder.Services.AddDbContexts(builder.Configuration);

// 跨域设置
CorsSetup.AddCorsSetup(builder);

var app = builder.Build();


app.UseResponseCaching();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    
}
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    var apiDescriptionGroups = app.Services.GetRequiredService<IApiDescriptionGroupCollectionProvider>().ApiDescriptionGroups.Items;
    foreach (var description in apiDescriptionGroups)
    {
        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName);
    }
});

app.UseCors(builder.Configuration.GetCorsOptions().Name);
//错误页
app.UseStatusCodePagesWithReExecute("/error-{0}");

// 认证
app.UseAuthentication();
// 授权
app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<JwtMiddleware>();
var errorRouteConfig = new ErrorRouteConfig(app);
errorRouteConfig.ConfigureErrorRoutes();

app.Run();
