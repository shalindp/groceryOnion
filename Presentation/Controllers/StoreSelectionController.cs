// using Microsoft.AspNetCore.Mvc;
// using Presentation.Requests;
// using Presentation.Responses;
//
// namespace Presentation.Controllers;
//
// [ApiController]
// [Route("[controller]")]
// public class StoreSelectionController : ControllerBase
// {
//
//     [HttpPost(Name = nameof(SelectStores))]
//     public async Task<CreateStoreSessionsResponse> SelectStores(
//         [FromBody] CreateStoreSessionsRequest request)
//     {
//       var result =   await _createStoreSessionsQuery.SendAsync(_mapper.Map(request));
//       return _mapper.Map(result);
//     }
// }