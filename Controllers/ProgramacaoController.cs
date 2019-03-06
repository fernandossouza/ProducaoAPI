using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProducaoAPI.Service.Interface;

namespace ProducaoAPI.Controllers
{
    [Route("api/[controller]")]
    public class ProgramacaoController : Controller
    {
        public readonly IProgramacaoService _ProgramacaoService;
        public  ProgramacaoController(IProgramacaoService programacaoService)
        {
            _ProgramacaoService = programacaoService;
        }

        // GET api/Programacao
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var programacaoList = await _ProgramacaoService.GetProgramacao();
                
                if(programacaoList != null && programacaoList.Count()>0)
                    return Ok(programacaoList);
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