using System.ComponentModel.Design.Serialization;
using System.Xml.Linq;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();


var productRepository = new ProductRepository();

app.MapGet("/", () => productRepository.GetAll());

app.MapGet("/products/{id}", (int id) =>
{
    var product = productRepository.Get(id);
    return Results.Ok(product);
});

app.MapPost("/products", (Product newProduct) =>
{
    var addedProduct = productRepository.Add(newProduct);
    return Results.Created($"/products/{addedProduct.Id}", addedProduct);
});

app.MapPut("/products/{id}", (int id, Product putProduct) =>
{
    putProduct.Id = id;
    var updated = productRepository.Update(putProduct);
    return Results.Ok(putProduct);
});

app.MapDelete("/products/delete/{id}", (int id) =>
{
    productRepository.Remove(id);
    return Results.NoContent();
});

app.Run();







public class ProductRepository
{
    private List<Product> products = new List<Product>();
    private int nextId = 1;

    public ProductRepository()
    {
        Add(new Product { Name = "Red Dead Redemption 2", Genre = "Шутер-приключения", Price = 2499 });
        Add(new Product { Name = "Metro 2033", Genre = "Шутер", Price = 569 });
        Add(new Product { Name = "S.T.A.L.K.E.R. 2", Genre = "Выживание-хоррор", Price = 3999 });
    }

    public IEnumerable<Product> GetAll()
    {
        return products;
    }

    public Product Get(int id)
    {
        return products.Find(p => p.Id == id);
    }

    public Product Add(Product item)
    {
        item.Id = nextId++;
        products.Add(item);
        return item;
    }

    public void Remove(int id)
    {
        products.RemoveAll(p => p.Id == id);
    }

    public bool Update(Product item)
    {
        int index = products.FindIndex(p => p.Id == item.Id);
        if (index == -1)
        {
            return false;
        }
        products.RemoveAt(index);
        products.Add(item);
        return true;
    }
}

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Genre { get; set; }
    public int Price { get; set; }
}