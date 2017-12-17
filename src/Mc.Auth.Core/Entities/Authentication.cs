namespace Mc.Auth.Core.Entities
{
    public class Authentication
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string SecretKey { get; set; }
        public double Expires { get; set; }
    }
}