using System.ComponentModel.DataAnnotations;
using static EventNotifier.IdentityServer.Validators.IdentityValidation;

namespace EventNotifier.IdentityServer.Models.Identity
{
    public class LoginRequestModel
    {
        [Required]
        [Email]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}
