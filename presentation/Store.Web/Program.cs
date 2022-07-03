using Store;
using Store.Contractors;
using Microsoft.EntityFrameworkCore;
using Store.Web.App;
using Store.Web.Contractors;
using Store.YandexKassa;
using Store.Data.EF;
using Store.Memory;

//������ ��������� ������  WebApplicationBuilder (����� ������������ ���- ����������)
var builder = WebApplication.CreateBuilder();
// ������� ������ ����������� ������� � ���� �������� � appsettings.json
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// ��������� �������� StoreDbContext � �������� ������� � ����������
builder.Services.AddDbContext<StoreDbContext>(options => options.UseSqlServer(connectionString));

// ����� �������� ����� ����� ����������� ��������� ����������� � ��� ���������� ASP.NET Core
// ���� AddControllersWithViews, ���������� MVC.
//��������� ������ ��� ������������ � ��������� ��������� IServiceCollection .
//���� ����� �� ����� �������������� ������, ������������ ��� �������.
builder.Services.AddControllersWithViews();

//���������� ASP.NET Core �������� ������ HttpContext����� ��������� IHttpContextAccessor
//� ��� ���������� �� ��������� HttpContextAccessor .
//��� ���������� ������������ ������ IHttpContextAccessor �����,
//����� ��� ����� ������ � HttpContext ���������� ����� ������.
builder.Services.AddHttpContextAccessor();
//��� ����� ����� ��� ��������������� ����������� � ASP.NET Core
//�������������� ��� � ��� ���, ��������� ������������ ����������� ��������� ����������,
//������� ������ �������������� ��� ������� ������ ��� �������� ����������,
//������� � ���� ����������.
builder.Services.AddDistributedMemoryCache();
// ��� ���������� ������� � ���������� � ASP.NET Core  ������ .AddSession
builder.Services.AddSession(options =>
{
    //������ �������� ������� �������� � appsettings.json.
    //��� �������� ����� �������������� ��� ��������� ������� �������� ������ � �����������.
    options.IdleTimeout = TimeSpan.FromSeconds(20);
    //�������� ��� ������ ��������, �����������, �������� �� ���� cookie ����������� ��������
    options.Cookie.HttpOnly = true;
    //���������, ��������� �� ���� ���� cookie ��� ���������� ������ ����������.
    //���� true, �� �������� �������� �������� ����� ������. �������� �� ��������� �������.
    options.Cookie.IsEssential = true;
});

// ������ � ������ ���������� �����  ����� ��������  ������������ 
// ��������� ��. 
builder.Services.AddEfRepositories(connectionString);






//�������� ������ � ������������� push-�����������
builder.Services.AddSingleton<INotificationService, DebugNotificationService>();

// �������� ������ � ������������� push-�����������
builder.Services.AddSingleton<IDeliveryService, PostamateDeliveryService>();
// �������� ������ � ������������� ��� ������ ���������
builder.Services.AddSingleton<IPaymentService, CashPaymentService>();
// �������� ������ � ������������� ��� ������ ������ ������
builder.Services.AddSingleton<IPaymentService, YandexKassaPaymentService>();

builder.Services.AddSingleton<IWebContractorService, YandexKassaPaymentService>();

//builder.Services.AddScoped<INotificationService, DebugNotificationService>();

//builder.Services.AddScoped<IDeliveryService, PostamateDeliveryService>();

//builder.Services.AddScoped<IPaymentService, CashPaymentService>();

//builder.Services.AddScoped<IPaymentService, YandexKassaPaymentService>();

//builder.Services.AddScoped<IWebContractorService, YandexKassaPaymentService>();





// �������� ������ � ������������� ��� ������ � Product ������������
//builder.Services.AddSingleton<ProductService>();
builder.Services.AddScoped<ProductService>();
// �������� ������ � ������������� ��� ������ � ��������
//builder.Services.AddSingleton<OrderService>();
builder.Services.AddScoped<OrderService>();
// ��� ����� ���-����������, ������������ ��� ��������� ��������� HTTP � ���������.
var app = builder.Build();
//���� ���� �� ��������
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseMiddleware<CounterMiddleware>(); ��� �����
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
