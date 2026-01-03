using EstoqueLiaTattoo.Data;
using EstoqueLiaTattoo.DTOs;
using EstoqueLiaTattoo.Models;
using EstoqueLiaTattoo.Services;
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
    public class MateriaisController : ControllerBase
    {
        private readonly IEstoqueService _service;

        public MateriaisController(IEstoqueService service) => _service = service;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MaterialResponseDTO>>> Get()
        {
            return Ok(await _service.ListarMateriaisAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MaterialResponseDTO>> Get(int id)
        {
            var dto = await _service.ObterMaterialPorIdAsync(id);
            return dto == null ? NotFound() : Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<Material>> Post(Material material)
        {
            var novo = await _service.CriarMaterialAsync(material);
            return CreatedAtAction(nameof(Get), new { id = novo.Id }, novo);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return await _service.DeletarMaterialAsync(id) ? NoContent() : BadRequest("Erro ao deletar.");
        }

        [HttpGet("criticos")]
        public async Task<ActionResult<IEnumerable<Material>>> GetCriticos() => Ok(await _service.ObterMateriaisCriticosAsync());
    }

}
