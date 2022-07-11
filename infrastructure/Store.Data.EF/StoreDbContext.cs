using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Data.EF
{
    public class StoreDbContext : DbContext
    {
        public DbSet<ProductDto> Products { get; set; }

        public DbSet<OrderDto> Orders { get; set; }

        public DbSet<OrderItemDto> OrderItems { get; set; }

        public StoreDbContext(DbContextOptions<StoreDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            BuildProducts(modelBuilder);
            BuildOrders(modelBuilder);
            BuildOrderItems(modelBuilder);
        }

        private void BuildOrderItems(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderItemDto>(action =>
            {
                action.Property(dto => dto.Price)
                      .HasColumnType("money");

                action.HasOne(dto => dto.Order)
                      .WithMany(dto => dto.Items)
                      .IsRequired();
            });
        }

        private static void BuildOrders(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderDto>(action =>
            {
                action.Property(dto => dto.CellPhone)
                      .HasMaxLength(25);

                action.Property(dto => dto.DeliveryUniqueCode)
                      .HasMaxLength(40);

                action.Property(dto => dto.DeliveryPrice)
                      .HasColumnType("money");

                action.Property(dto => dto.PaymentServiceName)
                      .HasMaxLength(40);

                action.Property(dto => dto.DeliveryParameters)
                      .HasConversion(
                          value => JsonConvert.SerializeObject(value),
                          value => JsonConvert.DeserializeObject<Dictionary<string, string>>(value))
                      .Metadata.SetValueComparer(DictionaryComparer);

                action.Property(dto => dto.PaymentParameters)
                      .HasConversion(
                          value => JsonConvert.SerializeObject(value),
                          value => JsonConvert.DeserializeObject<Dictionary<string, string>>(value))
                      .Metadata.SetValueComparer(DictionaryComparer);

                action.Property(dto => dto.img)
                     .HasMaxLength(40);
            });
        }

        private static readonly ValueComparer DictionaryComparer =
            new ValueComparer<Dictionary<string, string>>(
                (dictionary1, dictionary2) => dictionary1.SequenceEqual(dictionary2),
                dictionary => dictionary.Aggregate(
                    0,
                    (a, p) => HashCode.Combine(HashCode.Combine(a, p.Key.GetHashCode()), p.Value.GetHashCode())
                )
            );

        private static void BuildProducts(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductDto>(action =>
            {
                action.Property(dto => dto.Сategories)
                      .HasMaxLength(17)
                      .IsRequired();

                action.Property(dto => dto.Title)
                      .IsRequired();

                action.Property(dto => dto.Price)
                .HasColumnType("money");
                action.HasData(
                    new ProductDto
                    {
                        Id = 1,
                        Сategories = "Chees",
                        Manufacturer = "Podoksha",
                        Title = "Качотта",
                        Description = "Сыр качо́тта — итальянский сыр, относится к полумягким сортам. " +
                            "Его готовят из молока овец, буйволов, коз или коров. " +
                            "Такой сыр выдерживается от недели до нескольких месяцев, иногда дольше. " +
                            "Часто в качотту добавляют орехи и травы.",
                        Price = 150m,
                        img = "/img/Качотта_photo.jpg" ,
                    },
                    new ProductDto
                    {
                        Id = 2,
                        Сategories = "Chees",
                        Manufacturer = "Подносковье",
                        Title = "Адыгейский",
                        Description = "Наименование «адыгейский сыр» закрепилось от названия Республики Адыгея, " +
                            "черкесское население которой массово производит этот сыр, в том числе и на продажу." +
                            "Брендом «Адыгейский сыр» он стал в 1980 году, "+
                            "когда началось его массовое промышленное производство.В это время о нём публикуется статья в журнале «Молочная промышленность» за 1980 год:"+
                            "«Настоящий адыгейский сыр пахнет свежим молоком и полевыми цветами," +
                            "относится к диетическим продуктам питания и обладает высокой пищевой ценностью»[2],",
                        Price = 100m,
                        img = "/img/Адыгейский_photo.jpg",
                    },
                    new ProductDto
                    {
                        Id = 3,
                        Сategories = "Оборудование",
                        Manufacturer = "Podoksha",
                        Title = "Лира",
                        Description = "Предназначение лиры – разрезание сырного сгустка в процессе производства сыра. " +
                            "Лира для сыра позволяет это сделать буквально за пару движений, " +
                            "а главное идеально ровными кусочками (в отличие от обычной нарезки ножом).. ",
                        Price = 1200m,
                        img = "https://bifi.kz/d/lira.jpg",
                    },
                    new ProductDto
                    {
                        Id = 4,
                        Сategories = "Chees",
                        Manufacturer = "Лебедева",
                        Title = "Маскарпоне",
                        Description = "итальянский сливочный сыр. Происходит из региона Ломбардия. " +
                            "Считается, что впервые его стали делать в области между городками Лоди и Аббиатеграссо " +
                            "к юго-западу от Милана в конце XVI — начале XVII века. " +
                            "Часто используется в приготовлении чизкейков и других десертов.",
                        Price = 60m,
                        img = "https://n1s2.hsmedia.ru/cc/7e/26/cc7e267644f902633d4a348cd096090b/620x432_1_59ada42f231c8b1e6f47e87a7a991eaf@800x558_0x59f91261_2951699031394186123.jpeg",
                    },
                    new ProductDto
                    {
                        Id = 5,
                        Сategories = "Chees",
                        Manufacturer = "Podoksha",
                        Title = "Качокава́лло",
                        Description = "Качокава́лло - сырные мешочки. Вытяжной полутвердый сыр из коровьего молока с вызреванием более 1.5 -2 месяца.",
                        Price = 190m,
                        img = "/img/Качокавалла_photo.jpg",
                    },
                    new ProductDto
                    {
                        Id = 6,
                        Сategories = "Chees",
                        Manufacturer = "Podoksha",
                        Title = "Рикотта",
                        Description = "Мягкий творожный сыр двойной варки",
                        Price = 80m,
                        img = "https://tvoyproduct.ru/files/products/1671150/63a1d3978b620a2ef75a3dcdfae00cd1.png",
                    }
                );
            });
        }
    }
}
