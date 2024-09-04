using AutoMapper;
using DAM;
using DAM.DAM.Api.DTOs.Profiles;
using DAM.DAM.BLL.Interfaces;
using DAM.DAM.BLL.Services;
using DAM.DAM.DAL.Contexts;
using DAM.DAM.DAL.Interfaces;
using DAM.DAM.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "DAM API", Version = "v1" });
    c.SchemaFilter<EnumSchemaFilter>();
});

builder.Services.AddDbContext<DAMContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped<IFolderService, FolderService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IPermissionService, PermissionService>();

builder.Services.AddAutoMapper(typeof(FolderProfile), typeof(FileProfile));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Seed the database with initial data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<DAMContext>();
    context.Database.Migrate();

    SeedData.Initialize(context);
}

app.Run();
