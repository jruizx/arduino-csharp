using Arduino.Common.Application.Transaction;
using Arduino.Controllers.AutoMapper;
using Arduino.DomainModel.ProjectAggregate;
using Arduino.Infrastructure;
using Arduino.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<ArduinoContext>(x => x.UseSqlServer(connectionString));

// Add services to the container.
builder.Services.AddTransient<IProjectRepository, ProjectRepository>();
builder.Services.AddAutoMapper(typeof(ArduinoProfile));

builder.Services.AddMvc(setup =>
{
    setup.Filters.Add(typeof(TransactionFilter));
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.Run();