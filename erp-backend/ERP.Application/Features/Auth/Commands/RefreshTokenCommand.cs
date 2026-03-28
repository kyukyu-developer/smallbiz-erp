using MediatR;
using ERP.Application.DTOs.Auth;
using ERP.Application.DTOs.Common;

namespace ERP.Application.Features.Auth.Commands
{
    public class RefreshTokenCommand : IRequest<Result<LoginResponseDto>>
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}
