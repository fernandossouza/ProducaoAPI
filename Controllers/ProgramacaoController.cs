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

        // GET api/Programacao/Pessoa
        [HttpGet("Pessoa/")]
        public async Task<IActionResult> GetPessoa()
        {
            try
            {
                var pessoaList = await _ProgramacaoService.GetPessoasAlocacao();
                
                if(pessoaList != null && pessoaList.Count()>0)
                    return Ok(pessoaList);
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST api/Programacao/Pessoa/1
        [HttpPost("Pessoa/{loteId}")]
        public async Task<IActionResult> PostPessoa(long loteId,[FromBody] List<TbProgramacaoAlocacao> alocacaoList)
        {
            try
            {
                var programacao = await _ProgramacaoService.PostAlocacao(loteId,alocacaoList);
                
                if(programacao != null)
                    return Ok(programacao);
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // Delete api/Programacao/Pessoa/1
        [HttpDelete("Pessoa/{loteId}")]
        public async Task<IActionResult> PostPessoa(long loteId,[FromBody] List<long> pessoaList)
        {
            try
            {
                var programacao = await _ProgramacaoService.DeleteAlocacao(loteId,pessoaList);
                
                if(programacao)
                    return Ok(programacao);
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