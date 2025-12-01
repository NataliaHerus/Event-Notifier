using static EventNotifier.IdentityServer.Validators.IdentityValidation;
using System.ComponentModel.DataAnnotations;

namespace EventNotifier.IdentityServer.Models.Identity
{
    public class EditUserRequestModel
    {
        [Required]
        public string? Id { get; set; }

        [Required]
        [Name]
        public string? FirstName { get; set; }

        [Required]
        [Name]
        public string? LastName { get; set; }

        [Required]
        [Email]
        public string? Email { get; set; }
    }
}
