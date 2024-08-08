using System.ComponentModel.DataAnnotations;

namespace Resursko.Domain.Models;

public class ForgotPassword
{
    [Required, EmailAddress]
    public string? Email { get; set; }
}
