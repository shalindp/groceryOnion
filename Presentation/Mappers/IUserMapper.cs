using Application.Commands.User;
using Application.Queries;
using Application.Queries.User;
using Application.Services;
using Presentation.Requests;
using Presentation.Requests.Authentication;
using Presentation.Responses;
using Presentation.Responses.Authentication;
using Riok.Mapperly.Abstractions;

namespace Presentation.Mappers;

public interface IUserMapper
{
    SignInQueryRequest Map(SignInRequest source);
    SignInResponse Map(SignInQueryResponse source);

    CreateUserCommandRequest Map(SignUpRequest source);
    SignInResponse Map(CreateUserCommandResponse source);
    RefreshTokenQueryRequest Map(RefreshRequest source);
    SignInResponse Map(RefreshTokenQueryResponse source);
}

[Mapper]
partial class UserMapper : IUserMapper
{
    public partial SignInQueryRequest Map(SignInRequest source);
    public partial SignInResponse Map(SignInQueryResponse source);
    public partial CreateUserCommandRequest Map(SignUpRequest source);
    public partial SignInResponse Map(CreateUserCommandResponse source);
    public partial RefreshTokenQueryRequest Map(RefreshRequest source);
    public partial SignInResponse Map(RefreshTokenQueryResponse source);
}