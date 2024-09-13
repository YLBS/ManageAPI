
using Commons.ConfigOptions;
using Commons.Tool;
using Commons.Helper;
using Commons.Send;
using ManageNew.Authentication.JWT;
using ManageNew.Tool;
using ManageNew.ExceptionFilter;
using ManageNew.Extensions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.FileProviders;

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
builder.Services.AddScoped<TempletHelper>();
builder.Services.AddScoped<HtmlHelper>();
builder.Services.AddScoped<ManageUserCache>();
builder.Services.AddScoped<Mail>();
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
#pragma warning disable CS8604 // 引用类型参数可能为 null。
var configReader = new AppSet(app.Services.GetService<IConfiguration>());
#pragma warning restore CS8604 // 引用类型参数可能为 null。
app.UseCors(builder.Configuration.GetCorsOptions().Name);
//错误页
app.UseStatusCodePagesWithReExecute("/error-{0}");

// 认证
app.UseAuthentication();
// 授权
app.UseAuthorization();
//使用静态文件
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")),
    RequestPath = new PathString("/static")
});
app.MapControllers();

app.UseMiddleware<JwtMiddleware>();
var errorRouteConfig = new ErrorRouteConfig(app);
errorRouteConfig.ConfigureErrorRoutes();

app.Run();
