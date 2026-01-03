using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using EstoqueLiaTattoo.Data;
using EstoqueLiaTattoo.Services;
using EstoqueLiaTattoo.Services.Impl;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<EstoqueLiaTattooContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("EstoqueLiaTattooContext") 
    ?? throw new InvalidOperationException("Connection string 'EstoqueLiaTattooContext' not found.")));

// Add services to the container.

builder.Services.AddControllers();
    
builder.Services.AddScoped<IEstoqueService, EstoqueService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
