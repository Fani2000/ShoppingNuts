var builder = DistributedApplication.CreateBuilder(args);

// Add your backend service
var backend = builder.AddProject<Projects.backendApi>("backend");

// Get backend URL
var backendUrl = backend.GetEndpoint("http");

// Add frontend as a Node.js app that publishes as Docker
var frontend = builder.AddNpmApp("frontend", "../frontend", "start")
    .WithReference(backend)
    .WaitFor(backend)
    .WithEnvironment("ASPNETCORE_ENVIRONMENT", builder.Environment.EnvironmentName)
    .WithEnvironment("BackendApi__Url", backendUrl)
    .WithEnvironment("VITE_BACKEND_API_URL", backendUrl) // Also set Vite env var
    .WithExternalHttpEndpoints()
    .WithEndpoint(name: "http", port: 80, targetPort: 5173, scheme: "http", isExternal: true)
    .PublishAsDockerFile();

builder.Build().Run();