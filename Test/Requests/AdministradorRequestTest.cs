using System.Net;
using System.Text;
using System.Text.Json;
using minimal_api.Domain.ModelViews;
using minimal_api.Domain.DTOs;
using Test.Helpers;

namespace Test.Requests;

[TestClass]
public class AdministradorRequestTest
{
    [ClassInitialize]
    public static void ClassInit(TestContext ctx) => Setup.ClassInit(ctx);

    [ClassCleanup]
    public static void ClassCleanup() => Setup.ClassCleanup();

    [TestMethod]
    public async Task TestarGetSetPropriedades()
    {
        var loginDTO = new LoginDTO { Email = "adm@teste.com", Senha = "123456" };
        var content = new StringContent(JsonSerializer.Serialize(loginDTO), Encoding.UTF8, "application/json");

        var response = await Setup.client.PostAsync("/administradores/login", content);

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadAsStringAsync();
        var admLogado = JsonSerializer.Deserialize<AdministradorLogado>(
            result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.IsNotNull(admLogado?.Email);
        Assert.IsNotNull(admLogado?.Perfil);
        Assert.IsNotNull(admLogado?.Token);
    }
}
