using System.ComponentModel.DataAnnotations;

#nullable disable

namespace IdentityManager.Models;

public class ExternalLoginConfirmationModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Name{ get; set; }
}