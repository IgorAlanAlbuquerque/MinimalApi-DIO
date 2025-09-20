using minimal_api.Domain.Enums;

namespace minimal_api.Domain.DTOs;

public class AdministradorDTO
{
    public Perfil Perfil { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Senha { get; set; } = default!;
}