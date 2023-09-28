namespace OpenclassApp.GoogleSignIn;

using Internal;
using Internal.Context;
using Internal.Google;
using Internal.Handlers.Callback;
using Internal.Handlers.Exchange;
using Internal.Handlers.Initiation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

public static class GoogleSignInExtensions
{
    public static IServiceCollection AddGoogleSignIn(this IServiceCollection services,
                                                     Action<GoogleSignInOptions>
                                                         configureOptions)
    {
        services.Configure(configureOptions);
        services.AddScoped<InitiationHandler>();
        services.AddScoped<CallbackHandler>();
        services.AddScoped<CodeExchangeHandler>();
        services.AddSingleton<ContextSerializer>();
        services.AddSingleton<UriFactory>();
        services.AddHttpContextAccessor();
        services.AddDataProtection();
        return services;
    }

    public static IApplicationBuilder UseGoogleSignIn(this IApplicationBuilder builder)
    {
        builder.UseMiddleware<RoutingMiddleware>();
        return builder;
    }

    internal static string? GetQueryParameter(this HttpContext context, string key)
    {
        if (!context.Request.Query.ContainsKey(key))
        {
            return null;
        }
        var redirectUri = context.Request.Query[key][0];
        return string.IsNullOrEmpty(redirectUri) ? null : redirectUri;
    }

    internal static string GetHostOrigin(this HttpContext context)
    {
        var isHttps = context.Request.IsHttps;
        var host = context.Request.Host.Value;
        var hostOrigin = isHttps ? "https://" : "http://";
        hostOrigin += host;
        return hostOrigin;
    }
}
