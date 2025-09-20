namespace minimal_api.Domain.ModelViews;

public record AdministradorModelView
{
    public string Email { get; set; } = default!;
    
    public string Perfil { get; set; } = default!;
}