using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProducaoAPI.Models;
using ProducaoAPI.Service.Interface;

namespace ProducaoAPI.Controllers
{
    [Route("api/[controller]")]
    public class ApontamentoSapController : Controller
    {
        public readonly IApontamentoSapService _ApontamentoSapService;
        public  ApontamentoSapController(IApontamentoSapService apontamentoSapService)
        {
            _ApontamentoSapService = apontamentoSapService;
        }

        // GET api/apontamentoSap
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]string opNome = null, [FromQuery]string loteNome = null,[FromQuery]string tipo = null,
         [FromQuery]string status = null)
        {
            try
            {
                var apontamentoList = await _ApontamentoSapService.GetApontamento();
                
                if(apontamentoList != null && apontamentoList.Count()>0)
                    return Ok(apontamentoList);
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST api/apontamentoSap
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TbApontamentoSap apontamentoSap)
        {
            try
            {
                var apontamento = await _ApontamentoSapService.PostApontamento(apontamentoSap);
                
                if(apontamento != null)
                    return Ok(apontamento);
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE api/apontamentoSap/1
        [HttpDelete("{apontamentoId}")]
        public async Task<IActionResult> Delete(long apontamentoId)
        {
            try
            {
                var apontamento = await _ApontamentoSapService.CancelarApontamento(apontamentoId);
                
                if(apontamento)
                    return Ok();
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}