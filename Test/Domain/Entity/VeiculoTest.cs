using minimal_api.Domain.Entity;

namespace Test.Domain.Entity;

[TestClass]
public class VeiculoTest
{
    [TestMethod]
    public void TestarGetSetPropriedades()
    {
        var veiculo = new Veiculo();

        veiculo.Id = 1;
        veiculo.Nome = "eh um carro";
        veiculo.Marca = "chevrolet";
        veiculo.Ano = 2005;

        Assert.AreEqual(1, veiculo.Id);
        Assert.AreEqual("eh um carro", veiculo.Nome);
        Assert.AreEqual("chevrolet", veiculo.Marca);
        Assert.AreEqual(2005, veiculo.Ano);
    }
}