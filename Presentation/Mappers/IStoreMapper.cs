using Application.Commands.Stores;
using Application.Queries.Store;
using Presentation.Requests.Stores;
using Presentation.Responses.Stores;
using Riok.Mapperly.Abstractions;

namespace Presentation.Mappers;

public interface IStoreMapper
{
    SelectStoresCommandRequest Map(SelectStoresRequest source);
    IList<StoreResponse> Map(IList<StoreQueryResponse> source);
}

[Mapper]
public partial class StoreMapper : IStoreMapper
{
    public partial SelectStoresCommandRequest Map(SelectStoresRequest source);
    public partial IList<StoreResponse> Map(IList<StoreQueryResponse> source);
}