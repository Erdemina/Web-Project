using MySql.Data.MySqlClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Kökte (/) /Home.cshtml'e yönlendirme
app.MapGet("/", (context) =>
{
    context.Response.Redirect("/Home");
    return Task.CompletedTask;
});

app.MapRazorPages();

app.Run();
