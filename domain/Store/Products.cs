using System;

namespace Store;
public class Products
{
    public int Id { get; }
    public string Title { get; }
    public string Manufacturer { get; }
    public string Сategories { get; }
    public Products( int id, string name, string cat, string manufacturer)
    {
        Id = id;
        Title = name;
        Сategories = cat;
        Manufacturer = manufacturer;
    }

    internal static bool IsСategories(string s)
    {
        //Как получить доступ к репозиторию
        if (string.IsNullOrWhiteSpace(s.Replace("-", "")))
            return false;

        return true;
    }
}
