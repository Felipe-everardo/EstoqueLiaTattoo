using EstoqueLiaTattoo.DTOs;
using EstoqueLiaTattoo.Models;

namespace EstoqueLiaTattoo.Services;

public interface IMovimentacaoServico
{
    // 1. Processa a entrada/saída (Regra de negócio)
    Task<Movimentacao?> ProcessarMovimentacaoAsync(Movimentacao movimentacao);

    // 2. Lista o histórico completo para a tabela do Front-end
    Task<IEnumerable<MovimentacaoResponseDTO>> ListarHistoricoAsync();

    // 3. Busca uma única movimentação (Necessário para o CreatedAtAction e detalhes)
    Task<MovimentacaoResponseDTO?> ObterPorIdAsync(int id);
}
