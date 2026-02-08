using Application.Commands.Stores;
using Application.Queries.Store;
using Microsoft.AspNetCore.Mvc;
using Presentation.Mappers;
using Presentation.Requests.Stores;
using Presentation.Responses.Stores;

namespace Presentation.Controllers
{
    [ApiController]
    // [Authorize]
    [Route("[controller]")]
    public class StoreController : ControllerBase
    {
        private readonly IStoreMapper _mapper;
        private readonly SelectStoresCommand _selectStoresCommand;
        private readonly GetStoresQuery _getStoresQuery;

        public StoreController(IStoreMapper mapper, SelectStoresCommand selectStoresCommand, GetStoresQuery getStoresQuery)
        {
            _mapper = mapper;
            _selectStoresCommand = selectStoresCommand;
            _getStoresQuery = getStoresQuery;
        }
        
        [HttpGet(Name = nameof(StoresAsync))]
        public async Task<IList<StoreResponse>> StoresAsync()
        {
            var result = await _getStoresQuery.SendAsync();
            return _mapper.Map(result);
        }

        [HttpPost("select", Name = nameof(SelectStoresAsync))]
        public async Task<bool> SelectStoresAsync(
            [FromBody] SelectStoresRequest request)
        {
            var result = await _selectStoresCommand.SendAsync(_mapper.Map(request));
            return result;
        }
    }
}