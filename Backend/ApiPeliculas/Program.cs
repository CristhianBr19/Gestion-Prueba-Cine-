using ApiPeliculas.Controllers.Servicies;
using ApiPeliculas.Repository;
using Microsoft.EntityFrameworkCore;
using PracFullStack.Contexts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//Endpoins 
builder.Services.AddEndpointsApiExplorer();
//Suager 
builder.Services.AddSwaggerGen();
//configuracion de conexion con la base
//extraemos la cadena de conexion desde el otro archivo 
var conectionString = builder.Configuration.GetConnectionString("DefaultConnection");
//inyeccion de dependencias 
//cualquier controler podra usar la base de datos 
builder.Services.AddDbContext<MoviesContext>(options => options.UseNpgsql(conectionString));

//configuracion cors permite conexion con fronend 

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

builder.Services.AddScoped<PeliculaRepository>();
builder.Services.AddScoped<PeliculaService>();


var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();

        //iniciamos swagger
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseCors("AllowAngularApp");

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();


