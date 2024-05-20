using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Asp.Versioning.Conventions;
using LayoutManager;
using LayoutManager.Domain.UnitOfWork;
using LayoutManager.Helpers;
using LayoutManager.Infrastructure;
using LayoutManager.Infrastructure.UnitOfWork;
using LayoutManager.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
//var builder = WebApplication.CreateBuilder(new WebApplicationOptions
//{
//    EnvironmentName = Environments.Production
//});

// Add services to the container.
builder.Services.AddCors();
builder.Services.AddControllers();

builder.Services.AddApiVersioning(opt =>
{
    opt.DefaultApiVersion = new ApiVersion(1, 0);
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.ReportApiVersions = true;
    opt.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader(),
                                                    new HeaderApiVersionReader("x-api-version"),
                                                    new MediaTypeApiVersionReader("x-api-version"));
}).AddMvc(
                options =>
                {
                    // automatically applies an api version based on the name of
                    // the defining controller's namespace
                    options.Conventions.Add(new VersionByNamespaceConvention());
                }).AddApiExplorer(setup =>
{
    setup.GroupNameFormat = "'v'VVV";
    setup.SubstituteApiVersionInUrl = true;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient(typeof(ISecurityService), typeof(SecurityService));
builder.Services.AddTransient(typeof(IFolderService), typeof(FolderService));
builder.Services.AddTransient(typeof(IItemService), typeof(ItemService));
builder.Services.AddTransient(typeof(IPageService), typeof(PageService));

builder.Services.AddDbContext<LayoutContext>(
               x => x.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.Configure<Configuration>(builder.Configuration.GetSection("Configuration"));

var app = builder.Build();

var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions.Reverse())
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                description.GroupName.ToUpperInvariant());
        }
    });
}
app.UseCors(a=>a.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
