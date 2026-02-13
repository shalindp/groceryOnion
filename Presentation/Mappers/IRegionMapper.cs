using Application.Actions.Session;
using Presentation.Responses;
using Riok.Mapperly.Abstractions;

namespace Presentation.Mappers;

public interface IRegionMapper
{
    public IList<WoolworthSessionResponse> Map(IList<WoolworthsSessionAction> source);
}

[Mapper]
public partial class RegionMapper : IRegionMapper
{
   public partial IList<WoolworthSessionResponse> Map(IList<WoolworthsSessionAction> source);
}