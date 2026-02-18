namespace BFF.Options;

public class AuthOptions
{
    public const string SectionName = "AuthOptions";

    public string Authority { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;

    public string ResponseType { get; set; } = "code";
    public string ResponseMode { get; set; } = "query";

    public List<string> Scope { get; set; } = new();
}
