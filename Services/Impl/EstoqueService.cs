using EstoqueLiaTattoo.Data;
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

    public async Task<IEnumerable<Material>> ListarMateriaisAsync()
    {
        return await _context.Material
            //.Include(m => m.Categoria)
            .ToListAsync();
    }

    public async Task<Material?> ObterMaterialPorIdAsync(int id)
    {
        return await _context.Material
            //.Include(m => m.Categoria)
            .FirstOrDefaultAsync(m => m.Id == id);
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

    public async Task<IEnumerable<Material>> ObterMateriaisCriticosAsync()
    {
        return await _context.Material.Where(m => m.QuantidadeAtual <= m.QuantidadeMinima).ToListAsync();
    }

    // --- MÉTODOS DE MOVIMENTAÇÃO ---

    public async Task<Movimentacao?> ProcessarMovimentacaoAsync(Movimentacao movimentacao)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var material = await _context.Material.FindAsync(movimentacao.MaterialId);
            if (material == null) return null; // Retorna null se o material não existir

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

    public async Task<IEnumerable<Movimentacao>> ListarMovimentacoesAsync()
    {
        return await _context.Movimentacao.Include(m => m.Material).ToListAsync();
    }

    public async Task<Movimentacao?> ObterMovimentacaoPorIdAsync(int id)
    {
        return await _context.Movimentacao.FindAsync(id);
    }
}