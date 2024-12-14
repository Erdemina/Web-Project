using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using Web_Project.Data; // AppDbContext için namespace

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddRazorPages();

// Add session support
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Oturum süresi (30 dakika)
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true; // GDPR uyumu için gerekli
});

// MariaDB bağlantısını kontrol et
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

try
{
    using (var connection = new MySqlConnection(connectionString))
    {
        connection.Open();
        Console.WriteLine("Database bağlantısı başarılı!");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Database bağlantı hatası: {ex.Message}");
}

// Add AppDbContext for Entity Framework Core
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))); // MySQL/MariaDB bağlantısı

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Enable session
app.UseSession();

app.UseAuthorization();

// Kökte (/) /Home.cshtml'e yönlendirme
app.MapGet("/", (context) =>
{
    context.Response.Redirect("/Home");
    return Task.CompletedTask;
});

app.MapRazorPages();

app.Run();
