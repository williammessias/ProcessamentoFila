using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProcessamentoFila.Application.Interfaces.Services;
using ProcessamentoFila.Domain.DTO;
using Swashbuckle.Swagger.Annotations;

namespace Api.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    [Produces("application/json")]
    public class CotacaoController : ControllerBase
    {
        private readonly IMoedaAppService _service;
        public CotacaoController(IMoedaAppService service)
        {
            _service = service;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DadosMoedaRequestDto">Esse objeto contém uma lista contendo moedas e suas datas</param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddItemFila")]
        [SwaggerResponseRemoveDefaults]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddItemFila([FromBody] List<MoedaDto> request)
        {
            try
            {

                if(!ModelState.IsValid)
                    return StatusCode(StatusCodes.Status422UnprocessableEntity);

                var cliente = await _service.AdicionarMoedaFilaAsync(request);

                if(cliente)
                    return StatusCode(StatusCodes.Status201Created);

                return StatusCode(StatusCodes.Status503ServiceUnavailable);
            }
            catch (Exception e )
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        /// <summary>
        /// Esse serviço busca o ultimo objeto inserido na fila 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetItemFila")]
        [SwaggerResponseRemoveDefaults]
        [ProducesResponseType(typeof(DadosMoedaResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetItemFila()
        {
            try
            { 
                var moedaResult = await _service.PegarMoedaFilaAsync();
                if(moedaResult.DadosMoeda.Any())
                    return StatusCode(StatusCodes.Status200OK, moedaResult);

                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

    }
}
