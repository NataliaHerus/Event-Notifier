namespace EventNotifier.IdentityServer
{
    public class AppSettings
    {
        public string? Secret { get; set; }

        public string? ClientUrl { get; set; }

        public string? EventNotifierUrl { get; set; }

        public string? ResetPasswordPath { get; set; }
    }
}
