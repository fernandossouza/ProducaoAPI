using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace ProducaoAPI.Models.Repository
{
    public class TbApontamentoSapRepository
    {
        private string stringConnection;
        private readonly IConfiguration _configuration;

        public TbApontamentoSapRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            stringConnection = _configuration.GetConnectionString("ProducaoConnection");
        }
        public async Task<IEnumerable<TbApontamentoSap>> GetApontamento()
        {
            IEnumerable<TbApontamentoSap> apontamentoList; 
            string sSql = string.Empty;
            sSql = "SELECT * ";
            sSql += " FROM [SPI_TB_APONTAMENTO_SAP] ";
            sSql += " ORDER BY id DESC ";
            
            using(IDbConnection db = new SqlConnection(stringConnection)){                
               
                apontamentoList = await db.QueryAsync<TbApontamentoSap>(sSql);
            }
            return apontamentoList;
        }

        public async Task<IEnumerable<TbApontamentoSap>> GetApontamento(long apontamentoId)
        {
            IEnumerable<TbApontamentoSap> apontamentoList; 
            string sSql = string.Empty;
            sSql = "SELECT * ";
            sSql += " FROM [SPI_TB_APONTAMENTO_SAP] ";
            sSql += " WHERE [id] =" + apontamentoId.ToString();
            
            using(IDbConnection db = new SqlConnection(stringConnection)){                
               
                apontamentoList = await db.QueryAsync<TbApontamentoSap>(sSql);
            }
            return apontamentoList;
        }

        public async Task<TbApontamentoSap> Insert(TbApontamentoSap apontamentoSAP)
        {
            IEnumerable<long> insertRow;
            string sSql = "INSERT INTO [SPI_TB_APONTAMENTO_SAP] ([ordemProducaoId],[ordemProducao],[loteId],[lote],[tipo],[motivo],[status],[quantidade]) ";
            sSql +=  " VALUES (@ordemProducaoId,@ordemProducao,@loteId,@lote,@tipo,@motivo,@status,@quantidade) ";
            sSql +=  " SELECT @@IDENTITY";

            using (IDbConnection db = new SqlConnection(stringConnection))
            {
                insertRow = await db.QueryAsync<long>(sSql, apontamentoSAP);
            }

            if(insertRow == null || insertRow.Count() == 0)
                return null;
            
            apontamentoSAP.id = insertRow.FirstOrDefault();

            return apontamentoSAP;
        }

        public async Task<bool> CancelarApontamento(long apontamentoId)
        {
            int rowNumber;
            string sSql = "UPDATE [SPI_TB_APONTAMENTO_SAP] SET [status] = 'Cancelado'";
            sSql +=  " WHERE [id]="+apontamentoId.ToString()+" AND [status]='Não Enviado'";

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