using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

#nullable disable

namespace IdentityManager.Models;

public class ApplicationUserClass : IdentityUser
{
    [Required]
    public string Name { get; set; }
}