using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using ProducaoAPI.Models;
using ProducaoAPI.Models.Repository;
using ProducaoAPI.Service.Interface;

namespace ProducaoAPI.Service
{
    public class ProgramacaoService : IProgramacaoService
    {
        private readonly TbProgramacaoRepository _TbProgramacaoRepository;
        private readonly TbProgramacaoAlocacaoRepository _TbProgramacaoAlocacaoRepository;
        private readonly IIntegracaoAPIService _IntegracaoAPIService;
        
        public ProgramacaoService(TbProgramacaoRepository tbProgramacaoRepository,IIntegracaoAPIService integracaoAPIService,
        TbProgramacaoAlocacaoRepository tbProgramacaoAlocacaoRepository)
        {
            _TbProgramacaoRepository = tbProgramacaoRepository;
            _IntegracaoAPIService = integracaoAPIService;
            _TbProgramacaoAlocacaoRepository = tbProgramacaoAlocacaoRepository;
        }

        public async Task<bool> DeleteAlocacao(long loteId, List<long> pessoaIdList)
        {
            var programacao = await GetProgramacaoPorLoteId(loteId);

            if(programacao == null)
                throw new Exception("Programação não existem");

            foreach(var pessoaId in pessoaIdList)
            {
                if(! await _TbProgramacaoAlocacaoRepository.Delete(programacao.id,pessoaId))
                    throw new Exception("Erro ao tentar deletar alocação");
            }

            return true;
        }


        public async Task<IEnumerable<ApiPessoaCadastro>> GetPessoasAlocacao()
        {
            var(pessoaList,statusApi) = await _IntegracaoAPIService.GetPessoas();
            
            if(statusApi == HttpStatusCode.NotFound || statusApi == HttpStatusCode.InternalServerError)
                throw new Exception(" Erro na comunicação com a API de cadastro Pessoa, retorno: " + statusApi.ToString());
            
            var programacoesSemana = await GetProgramacao();
            List<TbProgramacaoAlocacao> pessoasAlocadas = new List<TbProgramacaoAlocacao>();

            if(programacoesSemana != null)
            {
                foreach(var programacao in programacoesSemana)
                {
                    if(programacao.alocacao.Count()>0)
                    pessoasAlocadas.AddRange(programacao.alocacao);
                }
            }

            foreach(var pessoaAlocada in pessoasAlocadas)
            {
                var pessoa = pessoaList.FirstOrDefault(p=>p.id == pessoaAlocada.pessoaId);
                if(pessoa != null)
                    pessoa.alocacao = pessoaAlocada;
            }
            return pessoaList;
        }

        public async Task<IEnumerable<TbProgramacao>> GetProgramacao()
        {
            IEnumerable<TbProgramacao> programacaoList;
            // Obtem o número da semana atual
            int numeroSemana = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            programacaoList = await _TbProgramacaoRepository.GetProgramacao(numeroSemana);

            return programacaoList;
        }

        public async Task<TbProgramacao> GetProgramacaoPorLoteId(long loteId)
        {
            TbProgramacao programacao;

            programacao = await _TbProgramacaoRepository.GetProgramacaoPorLoteId(loteId);

            return programacao;
        }

        public async Task<TbProgramacao> PostAlocacao(long loteId, List<TbProgramacaoAlocacao> alocacaoList)
        {
            var programacao = await GetProgramacaoPorLoteId(loteId);

            if(programacao == null)
            {
                var (lote,statusAPI) = await _IntegracaoAPIService.GetLotePorId(loteId);
                TbProgramacao programacaoInsert = new TbProgramacao();

                programacaoInsert.loteId = loteId;
                programacaoInsert.loteNome = lote.lote;
                programacaoInsert.semana = lote.semana;

                programacao = await _TbProgramacaoRepository.Insert(programacaoInsert);
            }    

            if(programacao == null)
                throw new Exception("Não foi possivel cadastrar a programação");

            var programacoesSemana = await GetProgramacao();
            List<TbProgramacaoAlocacao> pessoasAlocadas = new List<TbProgramacaoAlocacao>();

            if(programacoesSemana != null)
            {
                foreach(var p in programacoesSemana)
                {
                    if(p.alocacao.Count()>0)
                    pessoasAlocadas.AddRange(p.alocacao);
                }
            }

            bool pessoasJaAlocadas = false;
            foreach(var alocacao in alocacaoList)
            {
                var pessoaAlocada = _TbProgramacaoAlocacaoRepository.GetAlocacaoPorPessoaId(alocacao.pessoaId);
                if(pessoasAlocadas.Where(p=>p.pessoaId == alocacao.pessoaId).Count() > 0)
                {
                    pessoasJaAlocadas = true;
                    continue;
                }

                alocacao.programacaoId = programacao.id;

                if(await _TbProgramacaoAlocacaoRepository.Insert(alocacao)==null)
                    throw new Exception("Não foi possivel inserir a alocação no banco de dados");
            }

            if(pessoasJaAlocadas)
                throw new Exception("Existem pessoas que estão alocados em outro lote");

            return await GetProgramacaoPorLoteId(loteId);


        }
    }
}