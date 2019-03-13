using System.Collections.Generic;
using System.Threading.Tasks;
using ProducaoAPI.Models;

namespace ProducaoAPI.Service.Interface
{
    public interface IApontamentoSapService
    {
         Task<IEnumerable<TbApontamentoSap>> GetApontamento();
         Task<TbApontamentoSap> PostApontamento(TbApontamentoSap apontamentoSap);
         Task<bool> CancelarApontamento(long apontamentoId);
    }
}