using System;

namespace ProducaoAPI.Models
{
    public class TbProgramacaoAlocacao
    {
        public long id{get;set;}
        public long programacaoId{get;set;}
        public long pessoaId{get;set;}
        public string pessoaNome{get;set;}
        public string bancada{get;set;}
        public string bancadaLogado{get;set;}
        public DateTime data{get;set;}
    }
}