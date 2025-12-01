using System.ComponentModel.DataAnnotations;

namespace EventNotifier.IdentityServer.Models.Identity
{
    public class PasswordResetModel
    {

        [Required]
        public string? Password { get; set; }

        [Required]
        public string? ConfirmPassword { get; set; }
    }
}
