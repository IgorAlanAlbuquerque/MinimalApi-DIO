using minimal_api.Domain.DTOs;
using minimal_api.Domain.Entity;

namespace minimal_api.Domain.Interfaces;

public interface IAdministradorService
{
    Administrador? BuscarPorId(int id);
    Administrador Incluir(Administrador newAdministrador);
    List<Administrador> Listar(int? pagina);
    Administrador? Login(LoginDTO loginDTO);
}