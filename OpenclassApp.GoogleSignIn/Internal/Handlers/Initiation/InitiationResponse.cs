namespace OpenclassApp.GoogleSignIn.Internal.Handlers.Initiation;

internal class InitiationResponse
{
    public required string AuthorizationUri { get; set; }
    public required string ProofKey { get; set; }
}
