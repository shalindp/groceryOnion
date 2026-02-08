namespace Presentation.Requests.Stores;

public class SelectStoresRequest
{
    public string[] WoolworthStoreIds { get; init; } = [];

    public string[] PaknSaveStoreIds { get; init; } = [];
}