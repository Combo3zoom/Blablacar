using IdentityServer4.Models;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddIdentityServer()
    .AddInMemoryApiResources(new List<ApiResource>())
    .AddInMemoryIdentityResources(new List<IdentityResource>())
    .AddInMemoryApiScopes(new List<ApiScope>())
    .AddInMemoryClients(new List<Client>()) // сховище памяті для кліентів і користувачів
    .AddDeveloperSigningCredential(); // демонстраційний сертифікат підпису
//ми зареєстрували identity server
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();