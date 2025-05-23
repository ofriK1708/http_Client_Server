var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(8496);
});
builder.Services.AddControllers();
var app = builder.Build();

app.MapControllers();

app.Run();