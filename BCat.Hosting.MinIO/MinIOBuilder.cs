namespace Aspire.Hosting.MinIO;

public class MinIOBuilder
{
    public int? ApiPort { get; set; }
    public int? ConsolePort { get; set; }
    public string? AccessKey { get; set; }
    public string? SecretKey { get; set; }
    public string? DataVolumePath { get; set; }

    public MinIOBuilder WithPorts(int? apiPort = null, int? consolePort = null)
    {
        ApiPort = apiPort;
        ConsolePort = consolePort;
        return this;
    }

    public MinIOBuilder WithCredentials(string accessKey, string secretKey)
    {
        AccessKey = accessKey;
        SecretKey = secretKey;
        return this;
    }

    public MinIOBuilder WithDataVolume(string path)
    {
        DataVolumePath = path;
        return this;
    }
}