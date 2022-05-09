using Store.Data;
using System;

namespace Store;
public class Product
{
    private readonly ProductDto dto;

    public int Id => dto.Id;



    public string Title
    {
        get => dto.Title;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException(nameof(Title));

            dto.Title = value.Trim();
        }
    }

    public string Manufacturer
    {
        get => dto.Manufacturer;
        set => dto.Manufacturer = value?.Trim();
    }
    public string Сategories
    {
        get => dto.Сategories;
        set => dto.Сategories = value?.Trim();
    }
    public string Description
    {
        get => dto.Description;
        set => dto.Description = value;
    }
    public decimal Price
    {
        get => dto.Price;
        set => dto.Price = value;
    }
    public decimal Count { get; }
    public Product(ProductDto dto) 
    { 
        this.dto = dto;
    }

    public static bool IsСategories(string query)
    {
        if (string.IsNullOrEmpty(query)) return false;

        if (string.IsNullOrWhiteSpace(query.Replace("-", "")))
            return false;

        return  true;
    }

    public static class DtoFactory
    {
        public static ProductDto Create(string categories,
                                     string manufacturer,
                                     string title,
                                     string description,
                                     decimal price)
        {

            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException(nameof(title));

            return new ProductDto
            {
                Сategories = categories,
                Manufacturer = manufacturer?.Trim(),
                Title = title.Trim(),
                Description = description?.Trim(),
                Price = price,
            };
        }
    }

    public static class Mapper
    {
        public static Product Map(ProductDto dto) => new Product(dto);

        public static ProductDto Map(Product domain) => domain.dto;
    }
}
