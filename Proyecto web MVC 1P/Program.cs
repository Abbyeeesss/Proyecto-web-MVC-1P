using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Proyecto_web_MVC_1P.Interfaces;
using Proyecto_web_MVC_1P.Repositories;
using Proyecto_web_MVC_1P.Services;

var builder = WebApplication.CreateBuilder(args);

// Configurar Entity Framework
builder.Services.AddDbContext<ProyectoWebMVCP1Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ProyectoWebMVCP1Context") ?? throw new InvalidOperationException("Connection string 'ProyectoWebMVCP1Context' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add API controllers
builder.Services.AddControllers();

// Add API Explorer for Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register Repositories
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
builder.Services.AddScoped<ICompraRepository, CompraRepository>();

// Register Services
builder.Services.AddScoped<IUsuarioService, UsuarioService>();

// Add CORS policy for mobile app
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMobileApp", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    // Configure Swagger for development
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Use CORS
app.UseCors("AllowMobileApp");

app.UseAuthorization();

// Map MVC controllers
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Map API controllers
app.MapControllers();

app.Run();