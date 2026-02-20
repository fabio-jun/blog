using Blog.Data;
using Blog.Data.Repositories;
using Blog.Domain.Interfaces;
using Microsoft.EntityFrameworkCore; //Para UseNpgsql e AddDbContext

//Cria o construtor da aplicação, carrega appsettings.json, variáveis de ambiente, etc
var builder = WebApplication.CreateBuilder(args);

//Registro de serviços (Injeção de Dependência)

//Registra os controllers da api
builder.Services.AddControllers();

//Registra o AppDbContext no container de DI
builder.Services.AddDbContext<AppDbContext>(options =>
      options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

//Registra o suporte a Swagger      
builder.Services.AddOpenApi();

// Cria uma nova instância por HTTP Request
builder.Services.AddScoped<IUserRepository, UserRepository>();

//Constroi a aplicação
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
