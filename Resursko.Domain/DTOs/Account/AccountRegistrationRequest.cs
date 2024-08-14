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
    [RegularExpression(@"^(?=.*[A-Z])(?=.*[!@#$%^&*(),.?\"":{}|<>]).{7,}$", ErrorMessage = "Password must be at least 7 characters long and contain at least one special character and have one uppercase charachter.")]
    public string? Password { get; set; }

    [RegularExpression(@"^(?=.*[A-Z])(?=.*[!@#$%^&*(),.?\"":{}|<>]).{7,}$", ErrorMessage = "Password must be at least 7 characters long and contain at least one special character.")]
    [Required, DataType(DataType.Password), Compare(nameof(Password))]
    public string? ConfirmPassword {  get; set; } 
}
