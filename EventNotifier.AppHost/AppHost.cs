var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.EventNotifier_IdentityServer>("eventnotifieridentityserver");
builder.AddProject<Projects.EventNotifier>("eventnotifier");



builder.Build().Run();
