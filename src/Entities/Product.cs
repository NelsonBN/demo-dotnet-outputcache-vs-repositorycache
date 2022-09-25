namespace Demo.Entities;

public class Product
{
    public Guid Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public double Price { get; set; } = default!;
}
