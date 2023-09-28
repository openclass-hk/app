namespace OpenclassApp.GoogleSignIn.Internal.Context;

using Proofing;

internal class SignInContext
{
    public ProofKey? ProofKey { get; set; }
    public string? RedirectUri { get; set; }
    public string? SignedInUserId { get; set; }

    public static SignInContext Create()
    {
        return new SignInContext();
    }
}
