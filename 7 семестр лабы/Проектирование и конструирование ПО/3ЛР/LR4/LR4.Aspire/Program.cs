var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.LR4>("Cars");
builder.AddProject<Projects.LR4_Clients>("Clients");
builder.Build().Run();
