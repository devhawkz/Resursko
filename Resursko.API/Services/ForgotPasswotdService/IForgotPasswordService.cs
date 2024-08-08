using Resursko.Domain.DTOs;
namespace Resursko.API.Services.ForgotPasswotdService;

public interface IForgotPasswordService
{
    Task<ForgotPasswordResponse> ForgotPassword(ForgotPassword request);
}
