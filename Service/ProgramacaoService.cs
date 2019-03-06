using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using ProducaoAPI.Models;
using ProducaoAPI.Models.Repository;
using ProducaoAPI.Service.Interface;

namespace ProducaoAPI.Service
{
    public class ProgramacaoService : IProgramacaoService
    {
        private readonly TbProgramacaoRepository _TbProgramacaoRepository;
        public ProgramacaoService(TbProgramacaoRepository tbProgramacaoRepository)
        {
            _TbProgramacaoRepository = tbProgramacaoRepository;
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
    }
}