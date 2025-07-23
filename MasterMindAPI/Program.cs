using MasterMindDataAccess;
using MasterMindResources.Interfaces;
using MasterMindService;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(8080);
    options.ListenAnyIP(8081, listenOptions =>
    {
        listenOptions.UseHttps();
    });
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSingleton<string>(@"Data Source=..\MasterMindDB.db");
builder.Services.AddSingleton<ICharactersRepository, CharactersRepository>();
builder.Services.AddSingleton<ICharactersService, CharactersService>();
builder.Services.AddSingleton<IGameRepository, GameRepository>();
builder.Services.AddSingleton<IGameTypeRepository, GameTypeRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
