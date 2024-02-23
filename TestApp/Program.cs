using TestApp.ProgramStartUp;

WebApplication.CreateBuilder(args)
    .RegisterServices()
    .Inject()
    .Build()
    .SetupMiddleware()
    .Run();