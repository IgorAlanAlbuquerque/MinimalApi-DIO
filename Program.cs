using minimal_api.Infra.Db;
using minimal_api.Domain.DTOs;
using Microsoft.EntityFrameworkCore;
using minimal_api.Domain.Interfaces;
using minimal_api.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using minimal_api.Domain.ModelViews;
using minimal_api.Domain.Entity;
using minimal_api.Domain.Enums;

#region Builder
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IAdministradorService, AdministradorService>();
builder.Services.AddScoped<IVeiculoService, VeiculoService>();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DbContexto>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
        );
});

var app = builder.Build();
#endregion

#region Home
app.MapGet("/", () => Results.Json(new Home())).WithTags("Home");
#endregion

#region Administrador
app.MapPost("/administradores/login", ([FromBody] LoginDTO loginDTO, IAdministradorService administradorService) =>
{
    if (administradorService.Login(loginDTO) != null)
    {
        return Results.Ok("Login com sucesso");
    }
    else
    {
        return Results.Unauthorized();
    }
}).WithTags("Administrador");

app.MapPost("/administradores", ([FromBody] AdministradorDTO administradorDTO, IAdministradorService administradorService) =>
{
    var validacao = new ErrosDeValidacao
    {
        Mensagem = new List<string>()
    };

    if (string.IsNullOrEmpty(administradorDTO.Perfil.ToString()))
        validacao.Mensagem.Add("O perfil é obrigatório.");

    if (string.IsNullOrEmpty(administradorDTO.Email))
        validacao.Mensagem.Add("O email é obrigatório.");

    if (string.IsNullOrEmpty(administradorDTO.Senha))
        validacao.Mensagem.Add("A senha é obrigatória.");

    if (validacao.Mensagem.Count > 0)
        return Results.BadRequest(validacao);

    var newAdministrador = new Administrador
    {
        Perfil = administradorDTO.Perfil.ToString(),
        Email = administradorDTO.Email,
        Senha = administradorDTO.Senha
    };
    var adm = administradorService.Incluir(newAdministrador);

    return Results.Created($"/administradores/{adm.Id}", new AdministradorModelView
    {
        Perfil = adm.Perfil,
        Email = adm.Email
    });
}).WithTags("Administrador");

app.MapGet("/administradores", ([FromQuery] int? pagina, IAdministradorService administradorService) =>
{
    var adms = new List<AdministradorModelView>();
    var administradores = administradorService.Listar(pagina);
    foreach (var adm in administradores)
    {
        adms.Add(new AdministradorModelView
        {
            Perfil = adm.Perfil,
            Email = adm.Email
        });
    }
    return Results.Ok();
}).WithTags("Administrador");

app.MapGet("/admistradores/{id}", ([FromRoute] int id, IAdministradorService administradorService) =>
{
    var adm = administradorService.BuscarPorId(id);

    if (adm == null) return Results.NotFound();

    return Results.Ok(new AdministradorModelView
    {
        Perfil = adm.Perfil,
        Email = adm.Email
    });
}).WithTags("Administrador");
#endregion

#region Veiculos
ErrosDeValidacao validacaoDTO(VeiculoDTO veiculoDTO)
{
    var validacao = new ErrosDeValidacao
    {
        Mensagem = new List<string>()
    };

    if(string.IsNullOrEmpty(veiculoDTO.Nome))
        validacao.Mensagem.Add("O nome do veículo é obrigatório.");

    if(string.IsNullOrEmpty(veiculoDTO.Marca))
        validacao.Mensagem.Add("A marca do veículo é obrigatória.");

    if(veiculoDTO.Ano < 1886 || veiculoDTO.Ano > DateTime.Now.Year + 1)
        validacao.Mensagem.Add("O ano do veículo é inválido.");
    
    return validacao;
}

app.MapPost("/veiculos", ([FromBody] VeiculoDTO veiculoDTO, IVeiculoService veiculoService) =>
{
    var erros = validacaoDTO(veiculoDTO);
    if(erros.Mensagem.Count > 0)
        return Results.BadRequest(erros);

    var veiculo = new Veiculo
    {
        Nome = veiculoDTO.Nome,
        Marca = veiculoDTO.Marca,
        Ano = veiculoDTO.Ano
    };
    veiculoService.Incluir(veiculo);

    return Results.Created($"/veiculo/{veiculo.Id}", veiculo);
}).WithTags("Veiculo");

app.MapGet("/veiculos", ([FromQuery] int? pagina, IVeiculoService veiculoService) =>
{
    var veiculos = veiculoService.Todos(pagina);

    return Results.Ok(veiculos);
}).WithTags("Veiculo");

app.MapGet("/veiculos/{id}", ([FromRoute] int id, IVeiculoService veiculoService) =>
{
    var veiculo = veiculoService.BuscarPorId(id);

    if (veiculo == null) return Results.NotFound();

    return Results.Ok(veiculo);
}).WithTags("Veiculo");

app.MapPut("/veiculos/{id}", ([FromRoute] int id, VeiculoDTO veiculoDTO, IVeiculoService veiculoService) =>
{
    var erros = validacaoDTO(veiculoDTO);
    if(erros.Mensagem.Count > 0)
        return Results.BadRequest(erros);

    var veiculo = veiculoService.BuscarPorId(id);
    if (veiculo == null) return Results.NotFound();

    veiculo.Nome = veiculoDTO.Nome;
    veiculo.Marca = veiculoDTO.Marca;
    veiculo.Ano = veiculoDTO.Ano;

    veiculoService.Alterar(veiculo);

    return Results.Ok(veiculo);
}).WithTags("Veiculo");

app.MapDelete("/veiculos/{id}", ([FromRoute] int id, IVeiculoService veiculoService) =>
{
    var veiculo = veiculoService.BuscarPorId(id);
    if (veiculo == null) return Results.NotFound();

    veiculoService.Excluir(veiculo);

    return Results.NoContent();
}).WithTags("Veiculo");
#endregion

app.UseSwagger();
app.UseSwaggerUI();

app.Run();