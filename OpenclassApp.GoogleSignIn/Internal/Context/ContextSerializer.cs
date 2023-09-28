namespace OpenclassApp.GoogleSignIn.Internal.Context;

using Microsoft.AspNetCore.DataProtection;
using System.Text;
using System.Text.Json;

// TODO: Fix documentation

/// <summary>
///     Responsible for serializing and deserializing SignInSession instances, which are used to track
///     the state of an active Google Sign In session. These serialized representations of SignInSession
///     instances are passed in the URL between different hosts, so this service will use the Data
///     Protection API to ensure the serialized instances are encrypted for transit.
/// </summary>
internal class ContextSerializer
{
    private readonly IDataProtector _dataProtector;

    public ContextSerializer(IDataProtectionProvider dpProvider)
    {
        _dataProtector = dpProvider.CreateProtector("GoogleSignInSession");
    }

    /// <summary>
    ///     Serializes a SignInSession instance into a string.
    ///     The string will be protected with the DPAPI.
    /// </summary>
    /// <param name="context">The SignInSession instance to serialize.</param>
    /// <returns>A protected and serialized string representing the SignInSession instance.</returns>
    public string Serialize(SignInContext context)
    {
        var jsonString = JsonSerializer.Serialize(context);
        var byteArray = Encoding.UTF8.GetBytes(jsonString);
        var protectedByteArray = _dataProtector.Protect(byteArray);
        return Convert.ToBase64String(protectedByteArray);
    }

    /// <summary>
    ///     Deserializes a string into a SignInSession instance.
    /// </summary>
    /// <param name="serializedState">The serialized SignInSession instance.</param>
    /// <returns>
    ///     The SignInSession instance if deserialization succeeded,
    ///     or null if it didn't.
    /// </returns>
    public SignInContext? Deserialize(string serializedState)
    {
        if (string.IsNullOrEmpty(serializedState))
        {
            return null;
        }
        try
        {
            var protectedByteArray = Convert.FromBase64String(serializedState);
            var byteArray = _dataProtector.Unprotect(protectedByteArray);
            var jsonString = Encoding.UTF8.GetString(byteArray);
            var context = JsonSerializer.Deserialize<SignInContext>(jsonString);
            return context;
        }
        catch
        {
            return null;
        }
    }
}
