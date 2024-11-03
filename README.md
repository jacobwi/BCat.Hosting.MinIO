[![NuGet](https://img.shields.io/nuget/v/BCat.Hosting.MinIO.svg)](https://www.nuget.org/packages/BCat.Hosting.MinIO)
[![Downloads](https://img.shields.io/nuget/dt/BCat.Hosting.MinIO.svg)](https://www.nuget.org/packages/BCat.Hosting.MinIO)


# Aspire.Hosting.MinIO

MinIO integration for .NET Aspire applications. This package provides a simple way to add MinIO, an S3-compatible object storage server, to your Aspire application for managing object storage in development and production environments.

## Installation

In your AppHost project, install the .NET Aspire MinIO Hosting library with [NuGet](https://www.nuget.org):

```dotnetcli
dotnet add package BCat.Hosting.MinIO
```

## Usage

In your `Program.cs` or Aspire host project:

```csharp
var builder = DistributedApplication.CreateBuilder(args);

// Simple usage with default credentials
var minio = builder.AddMinIO("storage");

// With custom configuration
var minio = builder.AddMinIO("storage", options => options
    .WithPorts(apiPort: 9000, consolePort: 9001)
    .WithCredentials("myaccesskey", "mysecretkey")
    .WithDataVolume("/data/minio"));

// Reference in other projects
var api = builder.AddProject<Projects.MyApiProject>("api")
    .WithReference(minio);

// Access connection string in dependent projects
builder.AddProject<Projects.MyApiProject>("api")
    .WithReference(minio => minio
        .WithEnvironment("Storage__ConnectionString", minio.GetConnectionString()));
```

## Configuration Options

### Ports
- `ApiPort`: The port for the MinIO API server (default: 9000)
- `ConsolePort`: The port for the MinIO Console UI (default: 9001)

### Authentication
- `AccessKey`: Root user access key (default: "minioadmin")
- `SecretKey`: Root user secret key (default: "minioadmin")

### Storage
- `DataVolumePath`: Local path to persist MinIO data

## Connection String Format

The connection string format is:
```
http://{host}:{port};AccessKey={accessKey};SecretKey={secretKey}
```

## Endpoints

The MinIO container exposes two endpoints:
- API endpoint (default: 9000) - Used for S3 API operations
- Console endpoint (default: 9001) - Web UI for managing buckets and objects

## Examples

### Basic Configuration with AWS SDK

```csharp
// In your service project
var options = new AmazonS3Config
{
    ServiceURL = Configuration["Storage:ConnectionString"],
    ForcePathStyle = true
};

var credentials = new BasicAWSCredentials(
    Configuration["Storage:AccessKey"],
    Configuration["Storage:SecretKey"]);

services.AddSingleton<IAmazonS3>(_ => 
    new AmazonS3Client(credentials, options));
```

### Using with MinIO Client

```csharp
// In your service project
services.AddMinio(options =>
{
    options.EndPoint = Configuration["Storage:Host"];
    options.AccessKey = Configuration["Storage:AccessKey"];
    options.SecretKey = Configuration["Storage:SecretKey"];
    options.UseSSL = false;
});
```

## Default Credentials

If not specified, MinIO uses these default credentials:
- Access Key: `minioadmin`
- Secret Key: `minioadmin`

## Docker Image

This integration uses the official MinIO Docker image:
- Registry: `docker.io`
- Image: `minio/minio`
- Tag: `latest`

## Contributing

Issues and pull requests are welcome at https://github.com/dotnet/aspire
