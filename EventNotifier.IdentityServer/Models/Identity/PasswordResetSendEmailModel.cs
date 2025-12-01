using static EventNotifier.IdentityServer.Validators.IdentityValidation;
using System.ComponentModel.DataAnnotations;

namespace EventNotifier.IdentityServer.Models.Identity
{
    public class PasswordResetSendEmailModel
    {
        [Required]
        [Email]
        public string? Email { get; set; }
    }
}
