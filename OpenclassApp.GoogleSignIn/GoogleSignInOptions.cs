namespace OpenclassApp.GoogleSignIn;

public class GoogleSignInOptions
{
    public string GoogleAuthorizationBaseUrl { get; set; } =
        "https://accounts.google.com/o/oauth2/v2/auth";

    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public string InitiationEndpoint { get; set; } = "/auth/initiate";
    public string CallbackEndpoint { get; set; } = "/oauth/callback";
    public string CodeExchangeEndpoint { get; set; } = "/auth/exchange";
}
