using System;

namespace Store;
public class Chees
{
    public int Id { get; }
    public string Title { get; }

    public Chees( int id, string name)
    {
        Id = id;
        Title = name;
    }
}
