using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace ProducaoAPI.Models.Repository
{
    public class TbProgramacaoRepository
    {
        private string stringConnection;
        private readonly IConfiguration _configuration;

        public TbProgramacaoRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            stringConnection = _configuration.GetConnectionString("ProducaoConnection");
        }

        public async Task<IEnumerable<TbProgramacao>> GetProgramacao(int numeroSemana)
        {
            IEnumerable<TbProgramacao> programacao;
            string sSql = string.Empty;
            sSql = "SELECT P.*,A.* ";
            sSql += " FROM [SPI_TB_PROGRAMACAO] AS P ";
            sSql += " LEFT JOIN [SPI_TB_PROGRAMACAO_ALOCACAO] AS A ON P.id = A.programacaoId ";
            sSql += " WHERE P.semana =  " + numeroSemana.ToString();
            
            using(IDbConnection db = new SqlConnection(stringConnection)){                
               
                var lookup = new Dictionary<long,TbProgramacao>();
                db.Query<TbProgramacao,TbProgramacaoAlocacao,TbProgramacao>(sSql,(p,a) =>
                {
                TbProgramacao oProgramacao;
                if (!lookup.TryGetValue(p.id, out oProgramacao)) {
                         lookup.Add(p.id, oProgramacao = p);
                     }
                     if (oProgramacao.alocacao == null) 
                         oProgramacao.alocacao = new List<TbProgramacaoAlocacao>();
                    if(a!=null)
                        oProgramacao.alocacao.Add(a);

                     return oProgramacao;

                 }).AsQueryable();
                 programacao = lookup.Values;
            }

            if(programacao == null || programacao.Count()==0)
                return null;
            else
                return programacao;
        }

        public async Task<TbProgramacao> GetProgramacaoPorLoteId(long loteId)
        {
            TbProgramacao programacao;
            string sSql = string.Empty;
            sSql = "SELECT * ";
            sSql += " FROM [SPI_TB_PROGRAMACAO] ";
            sSql += " WHERE loteID =  " + loteId.ToString();
            
            using(IDbConnection db = new SqlConnection(stringConnection)){                
               
                programacao = await db.QueryFirstOrDefaultAsync<TbProgramacao>(sSql);
            }
                return programacao;
        }

        public async Task<TbProgramacao> Insert(TbProgramacao programacao)
        {
            IEnumerable<long> insertRow;
            string sSql = "INSERT INTO [dbo].[SPI_TB_PROGRAMACAO] ([loteId],[loteNome],[semana]) ";
            sSql +=  " VALUES (@loteId,@loteNome,@semana) ";
            sSql +=  " SELECT @@IDENTITY";

            using (IDbConnection db = new SqlConnection(stringConnection))
            {
                insertRow = await db.QueryAsync<long>(sSql, programacao);
            }

            if(insertRow == null || insertRow.Count() == 0)
                return null;
            
            programacao.id = insertRow.FirstOrDefault();

            return programacao;
        }
    }
}