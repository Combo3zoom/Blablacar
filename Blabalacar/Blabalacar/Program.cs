using System.Text.Json;
using System.Text.Json.Serialization;
using Blabalacar.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var opts = new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.IgnoreCycles };
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<BlalacarContext>();// передає у конктруктор наш контекст

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

