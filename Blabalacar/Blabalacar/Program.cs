using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Blabalacar.Database;
using Blabalacar.Models;
using Blabalacar.Repository;
using Blabalacar.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using  Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<IRegisterUserService, RegisterUserService>();
builder.Services.AddHttpContextAccessor();

//add repository object
builder.Services.AddScoped<IUserRepository<UserTrip, Guid>, UserRepository>();
builder.Services.AddScoped<IRepository<Trip, Guid>, TripRepository>();

builder.Services.AddSwaggerGen(options => // option swagger
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standart Authorization header using the Bearer scheme(\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey//?
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme) // option authentication
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JWT:Key").Value)),
            ValidateIssuer = false,//той хто видає
            ValidateAudience = false//той хто користується
        };
    });
builder.Services.AddIdentity<User, ApplicationRole>()
    .AddEntityFrameworkStores<BlalacarContext>();
builder.Services.AddDbContext<BlalacarContext>();// передає у конктруктор наш контекст
builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
    );

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthentication(); 
app.UseAuthorization();  

app.MapControllers();

app.Run();




