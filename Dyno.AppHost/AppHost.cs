var builder = DistributedApplication.CreateBuilder(args);

var dbServer = builder.AddPostgres("dyno").WithPgAdmin();
var db = dbServer.AddDatabase("sbd");

var apiService = builder.AddProject<Projects.Dyno_ApiService>("apiservice")
    .WithReference(db)
    .WithHttpHealthCheck("/health");

builder.AddProject<Projects.Dyno_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();