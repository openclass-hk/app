namespace OpenclassApp.GoogleSignIn.Internal.Google;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;

internal class UriFactory
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly GoogleSignInOptions _options;

    public UriFactory(IOptions<GoogleSignInOptions> options, IHttpContextAccessor contextAccessor)
    {
        _options = options.Value;
        _contextAccessor = contextAccessor;
    }

    public string CreateAuthorizationUri(string serializedContext)
    {
        var context = _contextAccessor.HttpContext!;
        var queryParams = new Dictionary<string, string?>
        {
            { "client_id", _options.ClientId },
            { "redirect_uri", context.GetHostOrigin() + _options.CallbackEndpoint },
            { "response_type", "code" },
            { "scope", "openid" },
            { "state", serializedContext }
        };
        return QueryHelpers.AddQueryString(_options.GoogleAuthorizationBaseUrl, queryParams);
    }
}
