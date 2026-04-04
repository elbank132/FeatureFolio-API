namespace FeatureFolio.Domain.Exceptions;

public class MissingJwtKeyException : Exception
{
    public MissingJwtKeyException() : base("JWT secret key is missing in environment variables.") { }
}
