
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
builder.Services.AddScoped<TempletHelper>();
builder.Services.AddScoped<HtmlHelper>();
builder.Services.AddScoped<ManageUserCache>();
builder.Services.AddScoped<Mail>();
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
#pragma warning disable CS8604 // �������Ͳ�������Ϊ null��
var configReader = new AppSet(app.Services.GetService<IConfiguration>());
#pragma warning restore CS8604 // �������Ͳ�������Ϊ null��
app.UseCors(builder.Configuration.GetCorsOptions().Name);
//����ҳ
app.UseStatusCodePagesWithReExecute("/error-{0}");

// ��֤
app.UseAuthentication();
// ��Ȩ
app.UseAuthorization();
//ʹ�þ�̬�ļ�
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
