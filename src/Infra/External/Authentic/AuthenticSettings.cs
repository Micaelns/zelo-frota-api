namespace Infra.External.Authentic;

public class AuthenticSettings
{
    public int SoftwareId { get; set; } = 0;
    public string URL { get; set; } = string.Empty;
    public string JwtSecret { get; set; } = string.Empty;
    public string JwtIssuer { get; set; } = string.Empty;
    public string JwtAudience { get; set; } = string.Empty;
}
