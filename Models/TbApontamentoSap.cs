using System;
using System.Collections.Generic;

namespace ProducaoAPI.Models
{
    public class TbApontamentoSap
    {
        public long id{get;set;}
        public long ordemProducaoId{get;set;}
        public string ordemProducao{get;set;}
        public long loteId{get;set;}
        public string lote{get;set;}
        public int quantidade{get;set;}
        public string tipo{get;set;}
        public string status{get;set;}
        public string motivo{get;set;}
        public DateTime? dataMigracao{get;set;}
        public ICollection<TbApontamentoSapRetorno> retornoSap{get;set;}
    }
}