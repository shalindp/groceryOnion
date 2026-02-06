using Application.Commands.Stores;
using Presentation.Requests;
using Presentation.Requests.Stores;
using Riok.Mapperly.Abstractions;

namespace Presentation.Mappers;

public interface IStoreMapper
{
    SelectStoresCommandRequest Map(SelectStoresRequest source);
}

[Mapper]
public partial class StoreMapper : IStoreMapper
{
    public partial SelectStoresCommandRequest Map(SelectStoresRequest source);
}