using EventAPI.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Pobieramy ConnectionString z appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// 2. Podłączamy bazę danych (PostgreSQL / Neon)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// 3. WAŻNE: Rejestrujemy Kontrolery (żeby API widziało Twoje pliki w folderze Controllers)
builder.Services.AddControllers();

// (Opcjonalnie) Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Konfiguracja HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

// 4. WAŻNE: Mapujemy kontrolery (uruchamiamy je)
app.MapControllers(); 

app.Run();