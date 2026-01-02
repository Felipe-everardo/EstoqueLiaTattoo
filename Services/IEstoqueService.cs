using EstoqueLiaTattoo.Models;

namespace EstoqueLiaTattoo.Services;

public interface IEstoqueService
{
    // Métodos de Material
    Task<IEnumerable<Material>> ListarMateriaisAsync();
    Task<Material?> ObterMaterialPorIdAsync(int id);
    Task<Material> CriarMaterialAsync(Material material);
    Task<bool> AtualizarMaterialAsync(Material material);
    Task<bool> DeletarMaterialAsync(int id);
    Task<IEnumerable<Material>> ObterMateriaisCriticosAsync();

    // Métodos de Movimentação
    Task<Movimentacao?> ProcessarMovimentacaoAsync(Movimentacao movimentacao);
    Task<IEnumerable<Movimentacao>> ListarMovimentacoesAsync();
    Task<Movimentacao?> ObterMovimentacaoPorIdAsync(int id);
}
