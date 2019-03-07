namespace ProducaoAPI.Models
{
    public class ApiPessoaCadastro
    {
        public int id { get; set; }
        public string matricula { get; set; }
        public string nome { get; set; }
        public string cargo { get; set; }
        public string tempoEmpresa { get; set; }
        public string imagem { get; set; }
        public TbProgramacaoAlocacao alocacao{get;set;}
    }
}