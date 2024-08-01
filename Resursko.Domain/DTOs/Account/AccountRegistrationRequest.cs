using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Resursko.Domain.DTOs.Account;

public class AccountRegistrationRequest
{
    [Required]
    public string? FirstName { get; set; }
    [Required]
    public string? LastName { get; set; }

    [Required]
    public string? Username {  get; set; }

    [Required, EmailAddress]
    public string? Email { get; set; }
    [Required, DataType(DataType.Password)]
    public string? Password { get; set; }

    [Required, DataType(DataType.Password), Compare(nameof(Password))]
    public string? ConfirmPassword {  get; set; }
}
