using Application.Commands.User;
using Application.Queries.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Mappers;
using Presentation.Requests.Authentication;
using Presentation.Responses.Authentication;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly SignInQuery _signInQuery;
        private readonly CreateUserCommand _createUserCommand;
        private readonly RefreshTokenQuery _refreshTokenQuery;
        private readonly IUserMapper _mapper;

        public UserController(SignInQuery signInQuery, CreateUserCommand createUserCommand, IUserMapper mapper, RefreshTokenQuery refreshTokenQuery)
        {
            _signInQuery = signInQuery;
            _createUserCommand = createUserCommand;
            _mapper = mapper;
            _refreshTokenQuery = refreshTokenQuery;
        }


        [AllowAnonymous]
        [HttpPost("sign-in", Name = nameof(SignInAsync))]
        public async Task<SignInResponse> SignInAsync(
            [FromBody] SignInRequest request)
        {
            var result = await _signInQuery.SendAsync(_mapper.Map(request));
            return _mapper.Map(result);
        }
        
        [AllowAnonymous]
        [HttpPost("sign-up", Name = nameof(SignUpAsync))]
        public async Task<SignInResponse> SignUpAsync(
            [FromBody] SignUpRequest request)
        {
            var result = await _createUserCommand.SendAsync(_mapper.Map(request));
            return _mapper.Map(result);
        }
        
        [AllowAnonymous]
        [HttpPost("refresh", Name = nameof(RefreshTokenAsync))]
        public async Task<SignInResponse> RefreshTokenAsync(
            [FromBody] RefreshRequest request)
        {
            var result = await _refreshTokenQuery.SendAsync(_mapper.Map(request));
            return _mapper.Map(result);
        }
    }
}