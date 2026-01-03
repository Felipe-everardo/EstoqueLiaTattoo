using EstoqueLiaTattoo.Data;
using EstoqueLiaTattoo.DTOs;
using EstoqueLiaTattoo.Models;
using EstoqueLiaTattoo.Services;
using EstoqueLiaTattoo.Services.Impl;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EstoqueLiaTattoo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovimentacoesController : ControllerBase
    {
        private readonly IEstoqueService _service;

        public MovimentacoesController(IEstoqueService estoqueService)
        {
            _service = estoqueService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovimentacaoResponseDTO>>> Get()
        {
            return Ok(await _service.ListarMovimentacoesAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MovimentacaoResponseDTO>> Get(int id)
        {
            var dto = await _service.ObterMovimentacaoPorIdAsync(id);
            return dto == null ? NotFound() : Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<Movimentacao>> PostMovimentacao(Movimentacao movimentacao)
        {
            var resultado = await _service.ProcessarMovimentacaoAsync(movimentacao);

            if (resultado == null)
            {
                return BadRequest("Erro ao processar movimentação. Verifique o estoque disponível ou o ID do material.");
            }

            // Retorna Status 201 (Created), o link para o GET do item e o próprio objeto
            return CreatedAtAction(nameof(Get), new { id = resultado.Id }, resultado);
        }
    }
}
