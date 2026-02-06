using Application.Commands.Stores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Mappers;
using Presentation.Requests.Stores;

namespace Presentation.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class StoreController : ControllerBase
    {
        private readonly IStoreMapper _mapper;
        private readonly SelectStoresCommand _selectStoresCommand;

        public StoreController(IStoreMapper mapper, SelectStoresCommand selectStoresCommand)
        {
            _mapper = mapper;
            _selectStoresCommand = selectStoresCommand;
        }

        [AllowAnonymous]
        [HttpPost("select", Name = nameof(SelectStoresAsync))]
        public async Task<bool> SelectStoresAsync(
            [FromBody] SelectStoresRequest request)
        {
            var result = await _selectStoresCommand.SendAsync(_mapper.Map(request));
            return result;
        }
    }
}