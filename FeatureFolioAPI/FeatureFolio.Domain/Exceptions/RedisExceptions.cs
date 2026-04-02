namespace FeatureFolio.Domain.Exceptions;

public class RedisInvalidDataException : Exception
{
    private const string _messageTemplate = "Either no data was found for key '{0}', or the data is invalid.";

    public RedisInvalidDataException(string key)
            : base(string.Format(_messageTemplate, key)) { }
}