using MediatR;
using ERP.Application.DTOs.Auth;
using ERP.Application.DTOs.Common;

namespace ERP.Application.Features.Auth.Commands
{
    public class LoginCommand : IRequest<Result<LoginResponseDto>>
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
