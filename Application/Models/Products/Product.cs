using Application.Enums.Products;

namespace Application.Models.Products;

public record Product(string Name, double Price, StoreType Store, string ImageUrl, string Region);
