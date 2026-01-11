using EstoqueLiaTattoo.DTOs;
using EstoqueLiaTattoo.Models;
using static EstoqueLiaTattoo.DTOs.TintaResponseDTO;

namespace EstoqueLiaTattoo.Services;

public interface ITintaService
{
    Task<List<TintaResponseDTO>> ListarItensEmUso();
    Task<bool> AbrirFrasco(AbrirTintaDTO dto);
    Task<bool> AtualizarPorcentagem(AtualizarConsumoDTO dto);
    Task<bool> DescartarItem(int id);
}
