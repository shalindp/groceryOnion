using Application.Models;
using Presentation.Responses;
using Riok.Mapperly.Abstractions;

namespace Presentation.Mappers;

public interface IRegionMapper
{
    public IList<CreateSessionWithRegionResponse> Map(IList<CreateSessionWithRegionDto> source);
}

[Mapper]
public partial class RegionMapper : IRegionMapper
{
   public partial IList<CreateSessionWithRegionResponse> Map(IList<CreateSessionWithRegionDto> source);
}