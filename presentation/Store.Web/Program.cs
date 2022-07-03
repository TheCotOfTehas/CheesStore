using Store;
using Store.Contractors;
using Microsoft.EntityFrameworkCore;
using Store.Web.App;
using Store.Web.Contractors;
using Store.YandexKassa;
using Store.Data.EF;
using Store.Memory;

//Создаю экзнмпляр класса  WebApplicationBuilder (Класс конструктора веб- приложений)
var builder = WebApplication.CreateBuilder();
// Получаю строку подключения которая у меня хранится в appsettings.json
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// добавляем контекст StoreDbContext в качестве сервиса в приложение
builder.Services.AddDbContext<StoreDbContext>(options => options.UseSqlServer(connectionString));

// Нужно добавить чтобы иметь возможность добавлять контроллеры в мое приложение ASP.NET Core
// Итак AddControllersWithViews, охватывает MVC.
//Добавляет службы для контроллеров в указанную коллекцию IServiceCollection .
//Этот метод не будет регистрировать службы, используемые для страниц.
builder.Services.AddControllersWithViews();

//Приложения ASP.NET Core получают доступ HttpContextчерез интерфейс IHttpContextAccessor
//и его реализацию по умолчанию HttpContextAccessor .
//Это необходимо использовать только IHttpContextAccessor тогда,
//когда вам нужен доступ к HttpContext внутренней части службы.
builder.Services.AddHttpContextAccessor();
//Эта штука нужна для Распределенного кэширования в ASP.NET Core
//Распределенный кеш — это кеш, совместно используемый несколькими серверами приложений,
//который обычно поддерживается как внешняя служба для серверов приложений,
//которые к нему обращаются.
builder.Services.AddDistributedMemoryCache();
// Для управление сеансом и состоянием в ASP.NET Core  делаем .AddSession
builder.Services.AddSession(options =>
{
    //Создаёт значение времени ожидания в appsettings.json.
    //Это значение будет использоваться для настройки времени ожидания сеанса и авторизации.
    options.IdleTimeout = TimeSpan.FromSeconds(20);
    //Получает или задает значение, указывающее, доступен ли файл cookie клиентскому сценарию
    options.Cookie.HttpOnly = true;
    //Указывает, необходим ли этот файл cookie для правильной работы приложения.
    //Если true, то проверки политики согласия можно обойти. Значение по умолчанию неверно.
    options.Cookie.IsEssential = true;
});

// Короче я сделал расширение чтобы  одной командой  подкдключить 
// несколько БД. 
builder.Services.AddEfRepositories(connectionString);






//добовляю сервис с инструментами push-уведомлений
builder.Services.AddSingleton<INotificationService, DebugNotificationService>();

// добовляю сервис с инструментами push-уведомлений
builder.Services.AddSingleton<IDeliveryService, PostamateDeliveryService>();
// добовляю сервис с инструментами для оплаты наличными
builder.Services.AddSingleton<IPaymentService, CashPaymentService>();
// добовляю сервис с инструментами для оплаты яндекс каасой
builder.Services.AddSingleton<IPaymentService, YandexKassaPaymentService>();

builder.Services.AddSingleton<IWebContractorService, YandexKassaPaymentService>();

//builder.Services.AddScoped<INotificationService, DebugNotificationService>();

//builder.Services.AddScoped<IDeliveryService, PostamateDeliveryService>();

//builder.Services.AddScoped<IPaymentService, CashPaymentService>();

//builder.Services.AddScoped<IPaymentService, YandexKassaPaymentService>();

//builder.Services.AddScoped<IWebContractorService, YandexKassaPaymentService>();





// добовляю сервис с инструментами для работы с Product репозиторием
//builder.Services.AddSingleton<ProductService>();
builder.Services.AddScoped<ProductService>();
// добовляю сервис с инструментами для работы с заказами
//builder.Services.AddSingleton<OrderService>();
builder.Services.AddScoped<OrderService>();
// тут создём веб-приложение, используемое для настройки конвейера HTTP и маршрутов.
var app = builder.Build();
//Ниже пока не разбирал
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseMiddleware<CounterMiddleware>(); Это тестю
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
