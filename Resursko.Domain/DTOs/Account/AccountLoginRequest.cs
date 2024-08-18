using System.ComponentModel.DataAnnotations;

namespace Resursko.Domain.DTOs.Account;

public class AccountLoginRequest
{
    [Required(ErrorMessage = "Email is required."), EmailAddress]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Password is required."), DataType(DataType.Password)]
    public string? Password { get; set; }
}
