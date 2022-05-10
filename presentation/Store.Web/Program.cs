using Store;
using Store.Contractors;
using Store.Memory;
using Store.Web.App;
using Store.Web.Contractors;
using Store.YandexKassa;
using Store.Data.EF;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// Add services to the container.
services.AddControllersWithViews();
services.AddHttpContextAccessor();
services.AddDistributedMemoryCache();
services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(20);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

//services.AddSingleton<IProductRepository, ProductReposetory>();
//services.AddSingleton<IOrderRepository, OrderRepository>();
services.AddEfRepositories(builder.Configuration.GetConnectionString("Store"));
services.AddSingleton<INotificationService, DebugNotificationService>();
services.AddSingleton<IDeliveryService, PostamateDeliveryService>();
services.AddSingleton<IPaymentService, CashPaymentService>();
services.AddSingleton<IPaymentService, YandexKassaPaymentService>();
services.AddSingleton<IWebContractorService, YandexKassaPaymentService>();
services.AddSingleton<ProductService>();
services.AddSingleton<OrderService>();

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
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapAreaControllerRoute(
    name: "yandex.kassa",
    areaName: "YandexKassa",
    pattern: "YandexKassa/{controller=Home}/{action=Index}/{id?}");

app.Run();
