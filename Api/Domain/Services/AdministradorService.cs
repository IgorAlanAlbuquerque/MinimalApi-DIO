using minimal_api.Domain.DTOs;
using minimal_api.Domain.Entity;
using minimal_api.Domain.Interfaces;
using minimal_api.Infra.Db;

namespace minimal_api.Domain.Services;

public class AdministradorService : IAdministradorService
{
    private readonly DbContexto _dbContexto;
    public AdministradorService(DbContexto db)
    {
        _dbContexto = db;
    }

    public Administrador? BuscarPorId(int id)
    {
        return _dbContexto.Administradores.Where(v => v.Id == id).FirstOrDefault();
    }

    public Administrador Incluir(Administrador newAdministrador)
    {
        _dbContexto.Administradores.Add(newAdministrador);
        _dbContexto.SaveChanges();

        return newAdministrador;
    }

    public List<Administrador> Listar(int? pagina)
    {
        if (pagina == null || pagina < 1)
        {
            pagina = 1;
        }
        var query = _dbContexto.Administradores.AsQueryable();

        int tamanhoPagina = 10;
        int pularRegistros = ((int)pagina - 1) * tamanhoPagina;

        return query.Skip(pularRegistros).Take(tamanhoPagina).ToList();
    }

    public Administrador Login(LoginDTO loginDTO)
    {
        return _dbContexto.Administradores.Where(a => a.Email == loginDTO.Email && a.Senha == loginDTO.Senha).FirstOrDefault();
    }
}
