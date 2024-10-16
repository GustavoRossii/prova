using API.Models;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDataContext>();
var app = builder.Build();

app.MapGet("/", () => "Projeto de Revisao com Tarefas a fazer!");

app.MapPost("/api/tarefas/cadastrar", ([FromBody]Tarefa tarefa, [FromServices] AppDataContext ctx) =>
{
    ctx.Tarefas.Add(tarefa);
    ctx.SaveChanges();
    return Results.Created("", tarefa);
} );

app.MapGet("/api/tarefas/buscar/{id}", ([FromRoute] int id, [FromServices] AppDataContext ctx) =>
{
    Tarefa? tarefa = ctx.Tarefas.Find(id);
    if (tarefa is null)
    {
        return Results.NotFound();
    }
    return Results.Ok(tarefa);
} );

app.MapGet("/api/tarefas/listar", ([FromServices] AppDataContext ctx) =>
{
    if(ctx.Tarefas.Any())
    {
        return Results.Ok(ctx.Tarefas.ToList());
    }
    return Results.NotFound();
} );

app.MapDelete("/api/tarefas/deletar/{id}", ([FromRoute] int id, [FromServices] AppDataContext ctx) =>
{
    Console.WriteLine($"Tentando deletar a tarefa com ID: {id}");
    Tarefa? tarefa = ctx.Tarefas.Find(id);
    if (tarefa == null)
    {
        Console.WriteLine("Tarefa não encontrada.");
        return Results.NotFound();
    }
    ctx.Tarefas.Remove(tarefa);
    ctx.SaveChanges();
    Console.WriteLine("Tarefa deletada com sucesso.");
    return Results.Ok(tarefa);
});

app.MapPut("/api/tarefas/alterar/{id}", ([FromBody]Tarefa tarefaAlterada, [FromRoute] int id, [FromServices] AppDataContext ctx) =>
{
    Tarefa? tarefa = ctx.Tarefas.Find(id);
    if (tarefa is null)
    {
        return Results.NotFound();
    }
    tarefa.Nome = tarefaAlterada.Nome;
    ctx.Tarefas.Update(tarefa);
    ctx.SaveChanges();
    return Results.Ok(tarefa);
});

app.Run();
