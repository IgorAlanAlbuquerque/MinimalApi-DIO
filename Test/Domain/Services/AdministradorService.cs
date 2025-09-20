using Microsoft.EntityFrameworkCore;
using minimal_api.Domain.Entity;
using minimal_api.Domain.Services;
using minimal_api.Infra.Db;

namespace Test.Domain.Entidades;

[TestClass]
public class AdministradorServiceTest
{
    private DbContexto CriarContextoDeTeste()
    {
        // Um banco novo por teste => estado sempre limpo, nada de TRUNCATE
        var options = new DbContextOptionsBuilder<DbContexto>()
            .UseInMemoryDatabase($"db-test-{Guid.NewGuid()}")
            .Options;

        return new DbContexto(options);
    }

    [TestMethod]
    public void TestandoSalvarAdministrador()
    {
        using var context = CriarContextoDeTeste();

        var adm = new Administrador
        {
            Email = "teste@teste.com",
            Senha = "teste",
            Perfil = "Adm"
        };

        var administradorService = new AdministradorService(context);

        administradorService.Incluir(adm);

        // Se Listar pagina, ajuste conforme retorno. Ex: Listar(1) retorna IEnumerable
        Assert.AreEqual(1, administradorService.Listar(1).Count());
    }

    [TestMethod]
    public void TestandoBuscarPorId()
    {
        using var context = CriarContextoDeTeste();

        var adm = new Administrador
        {
            Email = "teste@teste.com",
            Senha = "teste",
            Perfil = "Adm"
        };

        var administradorService = new AdministradorService(context);

        administradorService.Incluir(adm);

        // Se o serviço expõe BuscarPorId (síncrono):
        var admDoBanco = administradorService.BuscarPorId(adm.Id);

        // Se o método retorna anulável, use ?.Id (como você fez)
        Assert.AreEqual(adm.Id, admDoBanco?.Id);
    }
}
