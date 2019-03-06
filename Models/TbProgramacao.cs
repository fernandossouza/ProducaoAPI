using System.Collections.Generic;

namespace ProducaoAPI.Models
{
    public class TbProgramacao
    {
        public long id{get;set;}
        public long loteId{get;set;}
        public string loteNome{get;set;}
        public int semana{get;set;}
        public ICollection<TbProgramacaoAlocacao> alocacao{get;set;}
    }
}