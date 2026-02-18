namespace BackendApplication.Options;

public class AuthOptions
{
    public const string SectionName = "AuthOptions";

    public string Authority { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
}

