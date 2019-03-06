using System.Collections.Generic;
using System.Threading.Tasks;
using ProducaoAPI.Models;

namespace ProducaoAPI.Service.Interface
{
    public interface IProgramacaoService
    {
         Task<IEnumerable<TbProgramacao>> GetProgramacao();
         Task<TbProgramacao> GetProgramacaoPorLoteId(long loteId);
    }
}