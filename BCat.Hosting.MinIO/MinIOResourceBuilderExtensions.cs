using Aspire.Hosting.ApplicationModel;

namespace Aspire.Hosting.MinIO;

public static class MinIOResourceBuilderExtensions
{
    public static IResourceBuilder<MinIOResource> AddMinIO(
        this IDistributedApplicationBuilder builder,
        string name,
        Action<MinIOBuilder>? configure = null)
    {
        var options = new MinIOBuilder();
        configure?.Invoke(options);

        var resource = new MinIOResource(name, options.AccessKey, options.SecretKey);

        return builder.AddResource(resource)
            .WithImage(MinIOContainerImageTags.Image)
            .WithImageRegistry(MinIOContainerImageTags.Registry)
            .WithImageTag(MinIOContainerImageTags.Tag)
            .WithHttpEndpoint(
                targetPort: MinIOResource.DefaultApiPort,
                port: options.ApiPort,
                name: MinIOResource.ApiEndpointName)
            .WithEndpoint(
                targetPort: MinIOResource.DefaultConsolePort,
                port: options.ConsolePort,
                name: MinIOResource.ConsoleEndpointName)
            .ConfigureCredentials(options)
            .ConfigureVolume(options);
    }

    private static IResourceBuilder<MinIOResource> ConfigureCredentials(
        this IResourceBuilder<MinIOResource> builder,
        MinIOBuilder options)
    {
        return builder
            .WithEnvironment("MINIO_ROOT_USER", options.AccessKey ?? "minioadmin")
            .WithEnvironment("MINIO_ROOT_PASSWORD", options.SecretKey ?? "minioadmin");
    }

    private static IResourceBuilder<MinIOResource> ConfigureVolume(
        this IResourceBuilder<MinIOResource> builder,
        MinIOBuilder options)
    {
        if (!string.IsNullOrEmpty(options.DataVolumePath))
            builder = builder.WithVolume(options.DataVolumePath, "/data");
        return builder;
    }
}