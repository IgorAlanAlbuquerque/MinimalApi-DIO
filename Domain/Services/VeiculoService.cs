using System.Data.Common;
using minimal_api.Domain.DTOs;
using minimal_api.Domain.Entity;
using minimal_api.Domain.Interfaces;
using minimal_api.Infra.Db;

namespace minimal_api.Domain.Services;

public class VeiculoService : IVeiculoService
{
    private readonly DbContexto _dbContexto;
    public VeiculoService(DbContexto db)
    {
        _dbContexto = db;
    }
    public void Alterar(Veiculo veiculo)
    {
        _dbContexto.Veiculos.Update(veiculo);
        _dbContexto.SaveChanges();
    }

    public Veiculo? BuscarPorId(int id)
    {
        return _dbContexto.Veiculos.Where(v => v.Id == id).FirstOrDefault();
    }

    public void Excluir(Veiculo veiculo)
    {
        _dbContexto.Veiculos.Remove(veiculo);
        _dbContexto.SaveChanges();
    }

    public void Incluir(Veiculo veiculo)
    {
        _dbContexto.Veiculos.Add(veiculo);
        _dbContexto.SaveChanges();
    }

    public List<Veiculo> Todos(int? pagina = 1, string? nome = null, string? marca = null)
    {
        if (pagina == null || pagina < 1)
        {
            pagina = 1;
        }
        var query = _dbContexto.Veiculos.AsQueryable();

        if (!string.IsNullOrEmpty(nome))
        {
            query = query.Where(v => v.Nome.Contains(nome));
        }

        if (!string.IsNullOrEmpty(marca))
        {
            query = query.Where(v => v.Marca.Contains(marca));
        }

        int tamanhoPagina = 10;
        int pularRegistros = ((int)pagina - 1) * tamanhoPagina;

        return query.Skip(pularRegistros).Take(tamanhoPagina).ToList();
    }
}
