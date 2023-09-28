namespace OpenclassApp.GoogleSignIn.Internal.Handlers.Exchange;

using Context;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

internal class CodeExchangeHandler
{
    private readonly ContextSerializer _contextSerializer;

    public CodeExchangeHandler(ContextSerializer contextSerializer)
    {
        _contextSerializer = contextSerializer;
    }

    public async Task HandleRequestAsync(HttpContext httpContext)
    {
        var requestData = await httpContext.Request.ReadFromJsonAsync<CodeExchangeRequest>();
        if (requestData is null || string.IsNullOrEmpty(requestData.Code) ||
            string.IsNullOrEmpty(requestData.ProofKey))
        {
            httpContext.Response.StatusCode = 400;
            await httpContext.Response.WriteAsync("Bad request.");
            return;
        }
        var signInContext = _contextSerializer.Deserialize(requestData.Code);
        if (signInContext is null || requestData.ProofKey != signInContext.ProofKey!.Value)
        {
            httpContext.Response.StatusCode = 400;
            await httpContext.Response.WriteAsync("Bad request.");
            return;
        }

        var claimsPrincipal = new ClaimsPrincipal();
        var claimsIdentity = new ClaimsIdentity("google");
        claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier,
                                          signInContext.SignedInUserId!));
        claimsPrincipal.AddIdentity(claimsIdentity);
        await httpContext.SignInAsync(claimsPrincipal);
        await httpContext.Response.WriteAsync("Success.");
    }
}
