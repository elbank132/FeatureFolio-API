namespace FeatureFolio.Domain.Exceptions;

public class BlobNotFoundException : Exception
{
    private const string _messageTemplate = "Blobs not found in storage: ['{0}']";

    public BlobNotFoundException(string blobName)
            : base(string.Format(_messageTemplate, blobName)) { }

    public BlobNotFoundException(ICollection<string> blobNames)
        : 
        base(
            string.Format(
                _messageTemplate, 
                string.Join(", ", blobNames.Select(blob => $"'{blob}'")))
            ) { }
}
