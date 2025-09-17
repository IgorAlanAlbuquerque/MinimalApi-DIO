using minimal_api.Domain.DTOs;
using minimal_api.Domain.Entity;

namespace minimal_api.Domain.Interfaces;

public interface IVeiculoService
{
    List<Veiculo> Todos(int? pagina = 1, string? nome = null, string? marca = null);
    Veiculo? BuscarPorId(int id);
    void Incluir(Veiculo veiculo);
    void Alterar(Veiculo veiculo);
    void Excluir(Veiculo veiculo);

}