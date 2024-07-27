using Microsoft.EntityFrameworkCore;
using TiendaServicios.Api.CarritoCompra.Persistencia;
using MediatR;
using TiendaServicios.Api.CarritoCompra.RemoteInterface;
using TiendaServicios.Api.CarritoCompra.RemoteServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Obtener la cadena de conexión desde appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Configurar el contexto de la base de datos
builder.Services.AddDbContext<CarritoContexto>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddControllers();

// Registrar MediatR
builder.Services.AddMediatR(typeof(TiendaServicios.Api.CarritoCompra.Aplicacion.Consulta).Assembly);

// Registrar el servicio remoto de libros
builder.Services.AddHttpClient("Libros", config => {
    config.BaseAddress = new Uri(builder.Configuration["Services:Libros"]);
});
builder.Services.AddScoped<ILibroService, LibrosService>();

// Configurar CORS para permitir todas las solicitudes
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

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

// Usar CORS con la política configurada
app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
