using EstoqueLiaTattoo.Data;
using EstoqueLiaTattoo.DTOs;
using EstoqueLiaTattoo.Models;
using Microsoft.EntityFrameworkCore;
using static EstoqueLiaTattoo.DTOs.TintaResponseDTO;

namespace EstoqueLiaTattoo.Services.Impl;

public class TintaService : ITintaService
{
    public readonly EstoqueLiaTattooContext _context;

    public TintaService(EstoqueLiaTattooContext context)
    {
        _context = context;
    }

    public async Task<List<TintaResponseDTO>> ListarItensEmUso()
    {
        return await _context.Tinta
                .Include(t => t.Material)
                .Select(t => new TintaResponseDTO
                {
                    Id = t.Id,
                    TintaNome = t.Material.Nome, // Pega o nome lá da tabela de Materiais
                    Categoria = t.Material.Categoria != null ? t.Material.Categoria.Nome : "Sem Categoria",
                    PorcentagemRestante = t.PorcentagemRestante,
                    DataAbertura = t.DataAbertura
                })
                .ToListAsync();
    }

    public async Task<bool> AbrirFrasco(TintaResponseDTO.AbrirTintaDTO dto)
    {
        // 1. Acha o material no estoque (O "TintaId" do DTO é o ID do Material no estoque)
        var materialEstoque = await _context.Material.FindAsync(dto.TintaId);

        // Verifica se existe e se tem estoque
        if (materialEstoque == null || materialEstoque.QuantidadeAtual <= 0)
        {
            return false;
        }

        // 2. Diminui 1 do estoque lacrado
        materialEstoque.QuantidadeAtual -= 1;

        // 3. Cria a nova tinta aberta (Bancada)
        var novaTinta = new Tinta
        {
            MaterialId = dto.TintaId,
            PorcentagemRestante = 100, // Começa cheia
            DataAbertura = DateTime.Now
        };

        _context.Tinta.Add(novaTinta);

        // Salva as duas operações (Update no Material e Insert na Tinta)
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> AtualizarPorcentagem(AtualizarConsumoDTO dto)
    {
        // Busca a tinta aberta pelo ID dela (não do material)
        var tinta = await _context.Tinta.FindAsync(dto.Id);

        if (tinta == null) return false;

        tinta.PorcentagemRestante = dto.NovaPorcentagem;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DescartarItem(int id)
    {
        var tinta = await _context.Tinta.FindAsync(id);
        if (tinta == null) return false;

        _context.Tinta.Remove(tinta);
        await _context.SaveChangesAsync();
        return true;
    }

    
}
