namespace Mc.Auth.Core.Entities
{
    public class AuthorizationSettings
    {
        public string Issuer { get; set; }

        public string Audience { get; set; }

        public string SecretKey { get; set; }

        public double Expires { get; set; }
    }
}