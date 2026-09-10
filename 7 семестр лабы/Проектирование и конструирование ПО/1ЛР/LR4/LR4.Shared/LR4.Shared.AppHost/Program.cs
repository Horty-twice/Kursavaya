var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.LR4_Shared_ApiService>("apiservice");

builder.AddProject<Projects.LR4_Shared_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);

builder.Build().Run();
