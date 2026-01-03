using EstoqueLiaTattoo.Data;
using EstoqueLiaTattoo.DTOs;
using EstoqueLiaTattoo.Models;
using Microsoft.EntityFrameworkCore;

namespace EstoqueLiaTattoo.Services.Impl;

public class EstoqueService : IEstoqueService
{
    private readonly EstoqueLiaTattooContext _context;

    public EstoqueService(EstoqueLiaTattooContext context)
    {
        _context = context;
    }

    // --- MÉTODOS DE MATERIAL ---

    public async Task<IEnumerable<MaterialResponseDTO>> ListarMateriaisAsync()
    {
        return await _context.Material
            .Include(m => m.Categoria)
            .Select(material => new MaterialResponseDTO
            {
                Id = material.Id,
                Nome = material.Nome,
                QuantidadeAtual = material.QuantidadeAtual,
                QuantidadeMinima = material.QuantidadeMinima,
                PrecoUnitario = material.PrecoUnitario,
                NomeCategoria = material.Categoria != null ? material.Categoria.Nome : "Sem Categoria"
            })
            .ToListAsync();
    }

    public async Task<MaterialResponseDTO?> ObterMaterialPorIdAsync(int id)
    {
        var m = await _context.Material
            .Include(m => m.Categoria)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (m == null) return null;

        return new MaterialResponseDTO
        {
            Id = m.Id,
            Nome = m.Nome,
            QuantidadeAtual = m.QuantidadeAtual,
            QuantidadeMinima = m.QuantidadeMinima,
            PrecoUnitario = m.PrecoUnitario,
            NomeCategoria = m.Categoria != null ? m.Categoria.Nome : "Sem Categoria"
        };
    }

    public async Task<Material> CriarMaterialAsync(Material material)
    {
        _context.Material.Add(material);
        await _context.SaveChangesAsync();
        return material;
    }

    public async Task<bool> AtualizarMaterialAsync(Material material)
    {
        _context.Entry(material).State = EntityState.Modified;
        try
        {
            await _context.SaveChangesAsync();
            return true;
        }
        catch { return false; }
    }

    public async Task<bool> DeletarMaterialAsync(int id)
    {
        var material = await _context.Material.Include(m => m.Movimentacoes).FirstOrDefaultAsync(m => m.Id == id);
        if (material == null || material.Movimentacoes.Any()) return false;

        _context.Material.Remove(material);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<MaterialResponseDTO>> ObterMateriaisCriticosAsync()
    {
        return await _context.Material
            .Include(m => m.Categoria)
            .Where(m => m.QuantidadeAtual <= m.QuantidadeMinima)
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
    // --- MÉTODOS DE MOVIMENTAÇÃO ---

    public async Task<Movimentacao?> ProcessarMovimentacaoAsync(Movimentacao movimentacao)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var material = await _context.Material.FindAsync(movimentacao.MaterialId);
            if (material == null) return null;

            if (movimentacao.Tipo.ToLower() == "saida")
            {
                if (material.QuantidadeAtual < movimentacao.Quantidade) return null;
                material.QuantidadeAtual -= movimentacao.Quantidade;
            }
            else
            {
                material.QuantidadeAtual += movimentacao.Quantidade;
            }

            _context.Movimentacao.Add(movimentacao);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return movimentacao; // Retorna o objeto completo com o ID gerado
        }
        catch
        {
            await transaction.RollbackAsync();
            return null;
        }
    }

    public async Task<IEnumerable<MovimentacaoResponseDTO>> ListarMovimentacoesAsync()
    {
        return await _context.Movimentacao
            .Include(m => m.Material)
            .OrderByDescending(m => m.Data) // Histórico: mais recentes primeiro
            .Select(m => new MovimentacaoResponseDTO
            {
                Id = m.Id,
                MaterialId = m.MaterialId,
                NomeMaterial = m.Material != null ? m.Material.Nome : "Material Excluído",
                Quantidade = m.Quantidade,
                Tipo = m.Tipo,
                Data = m.Data,
                Observacao = m.Observacao
            }).ToListAsync();
    }

    public async Task<MovimentacaoResponseDTO?> ObterMovimentacaoPorIdAsync(int id)
    {
        var m = await _context.Movimentacao
            .Include(m => m.Material)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (m == null) return null;

        return new MovimentacaoResponseDTO
        {
            Id = m.Id,
            MaterialId = m.MaterialId,
            NomeMaterial = m.Material != null ? m.Material.Nome : "Material Excluído",
            Quantidade = m.Quantidade,
            Tipo = m.Tipo,
            Data = m.Data,
            Observacao = m.Observacao
        };
    }
}