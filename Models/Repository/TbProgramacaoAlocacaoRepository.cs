using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace ProducaoAPI.Models.Repository
{
    public class TbProgramacaoAlocacaoRepository
    {
        private string stringConnection;
        private readonly IConfiguration _configuration;

        public TbProgramacaoAlocacaoRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            stringConnection = _configuration.GetConnectionString("ProducaoConnection");
        }

        public async Task<TbProgramacaoAlocacao> GetAlocacaoPorPessoaId(long pessoaId)
        {
            TbProgramacaoAlocacao alocacao;
            string sSql = string.Empty;
            sSql = "SELECT * ";
            sSql += " FROM [SPI_TB_PROGRAMACAO_ALOCACAO] ";
            sSql += " WHERE pessoaId =  " + pessoaId.ToString();
            
            using(IDbConnection db = new SqlConnection(stringConnection)){                
               
                alocacao = await db.QueryFirstOrDefaultAsync<TbProgramacaoAlocacao>(sSql);
            }
                return alocacao;
        }

        public async Task<TbProgramacaoAlocacao> Insert(TbProgramacaoAlocacao alocacao)
        {
            IEnumerable<long> insertRow;
            string sSql = "INSERT INTO [SPI_TB_PROGRAMACAO_ALOCACAO] ([programacaoId],[pessoaId],[pessoaNome],[bancada],[data]) ";
            sSql +=  " VALUES (@programacaoId,@pessoaId,@pessoaNome,@bancada,GETDATE()) ";
            sSql +=  " SELECT @@IDENTITY";

            using (IDbConnection db = new SqlConnection(stringConnection))
            {
                insertRow = await db.QueryAsync<long>(sSql, alocacao);
            }

            if(insertRow == null || insertRow.Count() == 0)
                return null;
            
            alocacao.id = insertRow.FirstOrDefault();

            return alocacao;
        }

        public async Task<bool> Delete(long programacaoId, long pessoaId)
        {
            int rowNumber;
            string sSql = "DELETE [SPI_TB_PROGRAMACAO_ALOCACAO] ";
            sSql +=  " WHERE [programacaoId]="+programacaoId.ToString()+" AND [pessoaId]="+ pessoaId.ToString();

            using (IDbConnection db = new SqlConnection(stringConnection))
            {
                rowNumber = await db.ExecuteAsync(sSql);
            }

            if(rowNumber > 0 )
                return true;
            else
                return false;
        }
    }
}