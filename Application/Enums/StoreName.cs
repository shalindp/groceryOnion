namespace Application.Enums;

public enum StoreName
{
    PaknSave,
    NewWorld,
    Woolworths
}

public static class StoreNameExtensions
{
    public static string ToDescription(this StoreName store) =>
        store switch
        {
            StoreName.PaknSave => "paknsave",
            StoreName.NewWorld => "newworld",
            StoreName.Woolworths => "woolworths",
            _ => throw new Exception("Invalid store name")
        };
    
    public static StoreName ToStoreNameEnum(this string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Store name value cannot be null or empty", nameof(value));

        return value.ToLower() switch
        {
            "paknsave" => StoreName.PaknSave,
            "newworld" => StoreName.NewWorld,
            "woolworths" => StoreName.Woolworths,
            _ => throw new ArgumentException($"Invalid store name: {value}", nameof(value))
        };
    }
}