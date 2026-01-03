using EstoqueLiaTattoo.DTOs;
using EstoqueLiaTattoo.Models;

namespace EstoqueLiaTattoo.Services;


public interface IEstoqueService
{
    // Métodos que retornam dados para a tela mudam para DTO
    Task<IEnumerable<MaterialResponseDTO>> ListarMateriaisAsync();
    Task<MaterialResponseDTO?> ObterMaterialPorIdAsync(int id);
    Task<IEnumerable<MaterialResponseDTO>> ObterMateriaisCriticosAsync();

    // Métodos de ação (POST/PUT) podem continuar recebendo a Model ou o DTO de criação
    Task<Material> CriarMaterialAsync(Material material);
    Task<bool> AtualizarMaterialAsync(Material material);
    Task<bool> DeletarMaterialAsync(int id);

    // Movimentações (Leituras)
    Task<IEnumerable<MovimentacaoResponseDTO>> ListarMovimentacoesAsync();
    Task<MovimentacaoResponseDTO?> ObterMovimentacaoPorIdAsync(int id);

    // Movimentação (Escrita) - Recebe a Model e retorna a Model/DTO após criar
    Task<Movimentacao?> ProcessarMovimentacaoAsync(Movimentacao movimentacao);
}
