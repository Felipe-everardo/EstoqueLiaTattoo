using EstoqueLiaTattoo.Data;
using EstoqueLiaTattoo.DTOs;
using EstoqueLiaTattoo.Models;
using Microsoft.EntityFrameworkCore;

namespace EstoqueLiaTattoo.Services.Impl;

public class MaterialService : IMaterialService
{
    private readonly EstoqueLiaTattooContext _context;

    public MaterialService(EstoqueLiaTattooContext context) => _context = context;

    public async Task<IEnumerable<MaterialResponseDTO>> ListarTodosAsync()
    {
        return await _context.Material.Include(m => m.Categoria)
            .Select(m => new MaterialResponseDTO
            {
                Id = m.Id,
                Nome = m.Nome,
                QuantidadeAtual = m.QuantidadeAtual,
                QuantidadeMinima = m.QuantidadeMinima,
                PrecoUnitario = m.PrecoUnitario,
                NomeCategoria = m.Categoria != null ? m.Categoria.Nome : "Sem Categoria"
            }).ToListAsync();
    }

    public async Task<MaterialResponseDTO?> ObterPorIdAsync(int id)
    {
        var m = await _context.Material.Include(m => m.Categoria).FirstOrDefaultAsync(m => m.Id == id);
        if (m == null) return null;

        return new MaterialResponseDTO
        {
            Id = m.Id,
            Nome = m.Nome,
            QuantidadeAtual = m.QuantidadeAtual,
            QuantidadeMinima = m.QuantidadeMinima,
            PrecoUnitario = m.PrecoUnitario,
            NomeCategoria = m.Categoria?.Nome ?? "Sem Categoria"
        };
    }

    public async Task<IEnumerable<MaterialResponseDTO>> ListarCriticosAsync()
    {
        return await _context.Material.Include(m => m.Categoria)
            .Where(m => m.QuantidadeAtual <= m.QuantidadeMinima)
            .Select(m => new MaterialResponseDTO
            {
                Id = m.Id,
                Nome = m.Nome,
                QuantidadeAtual = m.QuantidadeAtual,
                QuantidadeMinima = m.QuantidadeMinima,
                PrecoUnitario = m.PrecoUnitario,
                NomeCategoria = m.Categoria.Nome
            }).ToListAsync();
    }

    public async Task<Material> CriarAsync(Material material)
    {
        _context.Material.Add(material);
        await _context.SaveChangesAsync();
        return material;
    }

    public async Task<bool> AtualizarAsync(Material material)
    {
        _context.Entry(material).State = EntityState.Modified;
        try { await _context.SaveChangesAsync(); return true; }
        catch { return false; }
    }

    public async Task<bool> DeletarAsync(int id)
    {
        var material = await _context.Material.Include(m => m.Movimentacoes).FirstOrDefaultAsync(m => m.Id == id);
        if (material == null || material.Movimentacoes.Any()) return false;

        _context.Material.Remove(material);
        await _context.SaveChangesAsync();
        return true;
    }
}
