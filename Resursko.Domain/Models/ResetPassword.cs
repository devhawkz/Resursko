using System.ComponentModel.DataAnnotations;

namespace Resursko.Domain.Models;
public class ResetPassword
{
    [Required, DataType(DataType.Password)]
    public string? Password { get; set; }

    [Required, DataType(DataType.Password), Compare(nameof(Password))]
    public string? ComparePassword { get; set; }

    public string? Email { get; set; }
    public string? ResetToken { get; set; }
}
