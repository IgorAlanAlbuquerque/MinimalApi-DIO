using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using minimal_api.Domain.Interfaces;
using Test.Mocks;

namespace Test.Helpers;

public static class Setup
{
    public static TestContext testContext = default!;
    public static WebApplicationFactory<Program> http = default!;
    public static HttpClient client = default!;

    public static void ClassInit(TestContext ctx)
    {
        testContext = ctx;

        http = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("Testing");

                builder.ConfigureServices(services =>
                {
                    // remove a implementação real (se existir)…
                    var existing = services.SingleOrDefault(d => d.ServiceType == typeof(IAdministradorService));
                    if (existing is not null) services.Remove(existing);

                    // …e registra o mock
                    services.AddScoped<IAdministradorService, AdministradorServiceMock>();
                });
            });

        client = http.CreateClient();
    }

    public static void ClassCleanup() => http.Dispose();
}
