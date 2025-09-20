using minimal_api.Domain.DTOs;
using minimal_api.Domain.Entity;
using minimal_api.Domain.Interfaces;

namespace Test.Mocks;

public class AdministradorServiceMock : IAdministradorService
{
    private static readonly List<Administrador> _dados = new()
    {
        new Administrador { Id = 1, Email = "adm@teste.com",    Senha = "123456", Perfil = "Adm"    },
        new Administrador { Id = 2, Email = "editor@teste.com", Senha = "123456", Perfil = "Editor" }
    };

    public List<Administrador> Listar(int? pagina)
    {
        // Como é mock, devolvemos tudo. Se quiser paginar de verdade, aplique Skip/Take.
        return _dados.ToList();
    }

    public Administrador Incluir(Administrador newAdministrador)
    {
        // gera Id simples só pro mock
        newAdministrador.Id = (_dados.Count == 0 ? 1 : _dados.Max(a => a.Id) + 1);
        _dados.Add(newAdministrador);
        return newAdministrador;
    }

    public Administrador? BuscarPorId(int id)
        => _dados.FirstOrDefault(a => a.Id == id);

    public Administrador? Login(LoginDTO loginDTO)
        => _dados.FirstOrDefault(a => a.Email == loginDTO.Email && a.Senha == loginDTO.Senha);
}
