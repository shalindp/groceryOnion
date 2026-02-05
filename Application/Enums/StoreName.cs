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
}