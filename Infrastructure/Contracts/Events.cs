namespace Products.Infrastructure.Contracts;


public record NewProduct(Guid ProductID, Guid ProductVariantId, string ProductName, string SKU, decimal UnitPrice);
public record UpdateProduct(Guid ProductID, Guid ProductVariantId, string ProductName, string SKU, decimal UnitPrice);
public record DeleteProduct(Guid ProductID, Guid ProductVariantId);
