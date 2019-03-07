using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using ProducaoAPI.Models;

namespace ProducaoAPI.Service.Interface
{
    public interface IIntegracaoAPIService
    {
        Task<(List<ApiPessoaCadastro>, HttpStatusCode)> GetPessoas();
        Task<(dynamic, HttpStatusCode)> GetLotePorId(long loteId);
    }
}