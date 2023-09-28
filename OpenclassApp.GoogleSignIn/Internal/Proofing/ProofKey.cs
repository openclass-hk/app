namespace OpenclassApp.GoogleSignIn.Internal.Proofing;

internal class ProofKey
{
    public required string Value { get; set; }

    public static ProofKey Generate()
    {
        var random = new Random();
        var randomNumber =
            random.Next(100000, 1000000);// Generates a random number between 100000 and 999999.
        var randomString = randomNumber.ToString();
        var proofKey = new ProofKey
        {
            Value = randomString
        };
        return proofKey;
    }
}
