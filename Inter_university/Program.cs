using Inter_university;

var builder = WebApplication.CreateBuilder(args);
//configuracion para forzar la lectura del archivo appsettings.json productivo
//builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

var startup = new Startup(builder.Configuration);

startup.ConfigureServices(builder.Services);

var app = builder.Build();

startup.Configure(app, app.Environment);

app.Run();