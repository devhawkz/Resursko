using System.ComponentModel.DataAnnotations;

namespace Resursko.Domain.DTOs.Account;

public class AccountLoginRequest
{
    [Required, EmailAddress]
    public string? Email { get; set; }

    [Required, DataType(DataType.Password)]
    public string? Password { get; set; }
}
