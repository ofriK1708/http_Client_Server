using calc_server;
using Serilog;
using Stopwatch = System.Diagnostics.Stopwatch;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()

    // Suppress framework logs
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
    .MinimumLevel.Override("System", Serilog.Events.LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore", Serilog.Events.LogEventLevel.Warning)

    // Console output
    .WriteTo.Console(restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
        outputTemplate: "{Timestamp:dd-MM-yyyy HH:mm:ss.fff} {Level:u3}: {Message} | request #{RequestId}{NewLine}")

    // File outputs
    .WriteTo.File("logs/requests.log",
        restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Debug,
        outputTemplate: "{Timestamp:dd-MM-yyyy HH:mm:ss.fff} {Level:u3}: {Message} | request #{RequestId}{NewLine}")

    .WriteTo.File("logs/stack.log",
        restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
        outputTemplate: "{Timestamp:dd-MM-yyyy HH:mm:ss.fff} {Level:u3}: {Message} | request #{RequestId}{NewLine}")

    .WriteTo.File("logs/independent.log",
        restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Debug,
        outputTemplate: "{Timestamp:dd-MM-yyyy HH:mm:ss.fff} {Level:u3}: {Message} | request #{RequestId}{NewLine}")

    .Enrich.FromLogContext()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(8496);
});
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
});

builder.Host.UseSerilog();
builder.Logging.ClearProviders(); 

var app = builder.Build();

app.Use(async (context, next) =>
{
    
    int requestNumber = RequestTracker.GetAndIncrementCounter();
    
    var sw = Stopwatch.StartNew();
    
    var path = context.Request.Path;
    var method = context.Request.Method.ToUpper();
    
    Log.ForContext("RequestId", requestNumber)
        .ForContext("SourceContext", "request-logger")
        .Information("Incoming request | #{RequestId} | resource: {Resource} | HTTP Verb {Method}", requestNumber, path, method);

    // Proceed with request
    await next();
    
    sw.Stop();
    var durationMs = sw.ElapsedMilliseconds;

    Log.ForContext("RequestId", requestNumber)
        .ForContext("SourceContext", "request-logger")
        .Debug("request #{RequestId} duration: {Duration}ms", requestNumber, durationMs);
});

app.MapControllers();

app.Run();

