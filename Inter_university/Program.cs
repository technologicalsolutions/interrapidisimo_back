using Inter_university;

var builder = WebApplication.CreateBuilder(args);
//configuracion para forzar la lectura del archivo appsettings.json productivo
//builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

var startup = new Startup(builder.Configuration);

startup.ConfigureServices(builder.Services);

WebApplication app = builder.Build();

startup.Configure(app, app.Environment);

/// Inicializar la base de datos al iniciar la aplicación
await startup.InitializeDatabase(app);

app.Run();