using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using minimal_api.Infra.Db; // seu DbContexto

// Usa o Program "parcial" do projeto Api
public class ApiFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // Força ambiente "Testing" (opcional)
        builder.UseEnvironment("Testing");

        // Substitui serviços para o cenário de teste (ex.: DbContext InMemory)
        builder.ConfigureServices(services =>
        {
            // Remove o DbContext "real" (Npgsql)
            var toRemove = services
                .Where(d => d.ServiceType == typeof(DbContextOptions<DbContexto>))
                .ToList();
            foreach (var d in toRemove) services.Remove(d);

            // Adiciona DbContext InMemory (rápido e sem precisar de appsettings)
            services.AddDbContext<DbContexto>(opt =>
                opt.UseInMemoryDatabase("api-tests"));
        });
    }
}
