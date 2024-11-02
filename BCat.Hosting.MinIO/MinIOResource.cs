using Aspire.Hosting.ApplicationModel;

namespace Aspire.Hosting.MinIO;

public class MinIOResource(string name, string? accessKey = null, string? secretKey = null)
    : ContainerResource(name), IResourceWithConnectionString
{
    internal const string ApiEndpointName = "api";
    internal const string ConsoleEndpointName = "console";
    internal const int DefaultApiPort = 9000;
    internal const int DefaultConsolePort = 9001;

    private EndpointReference? _apiReference;
    private EndpointReference? _consoleReference;

    private EndpointReference ApiEndpoint =>
        _apiReference ??= new EndpointReference(this, ApiEndpointName);

    private EndpointReference ConsoleEndpoint =>
        _consoleReference ??= new EndpointReference(this, ConsoleEndpointName);

    public ReferenceExpression ConnectionStringExpression =>
        ReferenceExpression.Create(
            $"http://{ApiEndpoint.Property(EndpointProperty.Host)}:{ApiEndpoint.Property(EndpointProperty.Port)};" +
            $"AccessKey={accessKey ?? "minioadmin"};" +
            $"SecretKey={secretKey ?? "minioadmin"}");

    public string? AccessKey { get; } = accessKey;
    public string? SecretKey { get; } = secretKey;
}