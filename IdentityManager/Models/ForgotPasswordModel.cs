using System.ComponentModel.DataAnnotations;

namespace IdentityManager.Models;

#nullable disable

public class ForgotPasswordModel
{
    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; }
}