using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using ProducaoAPI.Models;
using ProducaoAPI.Models.Repository;
using ProducaoAPI.Service.Interface;

namespace ProducaoAPI.Service
{
    public class ApontamentoSapService : IApontamentoSapService
    {
        private readonly TbApontamentoSapRepository _TbApontamentoSapRepository;
        private readonly IIntegracaoAPIService _IntegracaoAPIService;
        public ApontamentoSapService(TbApontamentoSapRepository tbApontamentoSapRepository,IIntegracaoAPIService integracaoAPIService)
        {
            _TbApontamentoSapRepository = tbApontamentoSapRepository;
            _IntegracaoAPIService = integracaoAPIService;
        }

        public async Task<bool> CancelarApontamento(long apontamentoId)
        {
            var apontamento = await _TbApontamentoSapRepository.GetApontamento(apontamentoId);

            if(apontamento == null)
                throw new System.Exception("Apontamento não encontrado no banco de dados ");

            return await _TbApontamentoSapRepository.CancelarApontamento(apontamentoId);
        }

        public async Task<IEnumerable<TbApontamentoSap>> GetApontamento()
        {
            IEnumerable<TbApontamentoSap> apontamentoSapList;

            apontamentoSapList = await _TbApontamentoSapRepository.GetApontamento();

            return apontamentoSapList;
        }

        public async Task<TbApontamentoSap> PostApontamento(TbApontamentoSap apontamentoSap)
        {
            if(apontamentoSap.tipo.ToLower() != "i" && apontamentoSap.tipo.ToLower() != "e")
                throw new System.Exception("São aceitos apenas tipo: I ou tipo: E ");

            var (lote,statusApi) = await _IntegracaoAPIService.GetLotePorId(apontamentoSap.loteId);

            if(statusApi == HttpStatusCode.NotFound || statusApi == HttpStatusCode.InternalServerError)
                    throw new System.Exception(" Erro na comunicação com a API de cadastro Pessoa, retorno: " + statusApi.ToString());

            if(lote == null)
                throw new System.Exception("Lote não encontrado");

            if((long)lote.ordemProducaoId != apontamentoSap.ordemProducaoId)
                throw new System.Exception("Id da ordem de produção é diferente do id identificado no lote.");

            apontamentoSap.status = "Não Enviado";

            apontamentoSap = await _TbApontamentoSapRepository.Insert(apontamentoSap);

            return apontamentoSap;
        }
    }
}