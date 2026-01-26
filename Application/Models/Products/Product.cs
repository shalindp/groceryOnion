using Application.Enums;

namespace Application.Models.Products;

public record Product(string Name, double Price, StoreType Store, string ImageUrl, string Region);
