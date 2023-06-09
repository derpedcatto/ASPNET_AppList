using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using shop.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Налагодження сесій:
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromSeconds(60);
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true;
});

// Add Data Context
builder.Services.AddDbContext<DataContext>(options =>
	options.UseMySql(builder.Configuration.GetConnectionString("PlanetDb"),
					 new MySqlServerVersion(new Version(8, 0, 23)),
                     serverOptions => serverOptions.MigrationsHistoryTable(tableName: HistoryRepository.DefaultTableName, schema: "ASP121")
                                                   .SchemaBehavior(MySqlSchemaBehavior.Translate, (schema, table) => $"{schema}_{table}")
));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Включення сесій, порядок має значення
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
