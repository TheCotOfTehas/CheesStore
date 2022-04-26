using System;

namespace Store;
public class Product
{
    public int Id { get; }
    public string Title { get; }
    public string Manufacturer { get; }
    public string Сategories { get; }
    public string Description { get; }
    public decimal Price { get; }
    public Product( int id, string name, string cat, string manufacturer, string description, decimal prise)
    {
        Id = id;
        Title = name;
        Сategories = cat;
        Manufacturer = manufacturer;
        Description = description;
        Price = prise;
    }

    internal static bool IsСategories(string query)
    {
        if (string.IsNullOrEmpty(query)) return false;

        if (string.IsNullOrWhiteSpace(query.Replace("-", "")))
            return false;

        return  true;
    }
}
