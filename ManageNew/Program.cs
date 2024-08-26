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

#region ȫ���쳣������

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ExceptionFilter>();
});
#endregion

//builder.Services.AddScoped<ITest, Test>();




builder.Services.AddScoped<ITokenService, TokenService>();

//����ע��ӿں�ʵ����
builder.Services.AddDi();
builder.Services.AddScoped<CheckPermission>();
//��ӻ���
builder.Services.AddResponseCaching(); //ֻ������get����
builder.Services.AddMemoryCache();

builder.Services.AddControllers();

#region JWT��֤��Ȩ
//��ʼ��JwtAuthOptions
builder.Services.Configure<JwtAuthOptions>(builder.Configuration.GetSection("JwtAuth"));

builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddJwtAuthorization();
#endregion

builder.Services.AddEndpointsApiExplorer();

//ʵ��ӳ������
var mapper = AutoMapperConfig.ConfigureAutoMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddSwaggerSetup();
builder.Services.AddDbContexts(builder.Configuration);

// ��������
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
//����ҳ
app.UseStatusCodePagesWithReExecute("/error-{0}");

// ��֤
app.UseAuthentication();
// ��Ȩ
app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<JwtMiddleware>();
var errorRouteConfig = new ErrorRouteConfig(app);
errorRouteConfig.ConfigureErrorRoutes();

app.Run();
