namespace OpenclassApp.GoogleSignIn.Internal;

using Handlers.Callback;
using Handlers.Exchange;
using Handlers.Initiation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

/// <summary>
///     This middleware will short circuit any requests to endpoints that pertain to the Google Sign In
///     flow and handle them appropriately.
/// </summary>
internal class RoutingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly GoogleSignInOptions _options;

    public RoutingMiddleware(RequestDelegate next, IOptions<GoogleSignInOptions> options)
    {
        _next = next;
        _options = options.Value;
    }

    public async Task InvokeAsync(HttpContext context, InitiationHandler initiationHandler,
                                  CallbackHandler callbackHandlerHandler,
                                  CodeExchangeHandler exchangeHandler)
    {
        if (context.Request.Path.Equals(_options.InitiationEndpoint))
        {
            await initiationHandler.HandleRequestAsync(context);
            return;
        }

        if (context.Request.Path.Equals(_options.CallbackEndpoint))
        {
            await callbackHandlerHandler.HandleRequestAsync(context);
            return;
        }

        if (context.Request.Method.Equals("POST") &&
            context.Request.Path.Equals(_options.CodeExchangeEndpoint))
        {
            await exchangeHandler.HandleRequestAsync(context);
            return;
        }

        await _next(context);
    }
}
