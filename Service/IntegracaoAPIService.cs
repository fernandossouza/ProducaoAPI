using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ProducaoAPI.Models;
using ProducaoAPI.Service.Interface;

namespace ProducaoAPI.Service
{
    public class IntegracaoAPIService : IIntegracaoAPIService
    {
        private HttpClient client;
        private IConfiguration _configuration;
        public IntegracaoAPIService(IConfiguration configuration)
        {
            client = new HttpClient();
            _configuration = configuration;
        }

        public async Task<(dynamic, HttpStatusCode)> GetLotePorId(long loteId)
        {
            dynamic loteReturn = null;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var builder = new UriBuilder(_configuration["loteServiceEndpoint"]+"/"+loteId.ToString());
            string url = builder.ToString();
            var result = await client.GetAsync(url);
            switch (result.StatusCode)
            {
                case HttpStatusCode.OK:
                    loteReturn = JsonConvert.DeserializeObject<dynamic>(await client.GetStringAsync(url));
                    return (loteReturn, HttpStatusCode.OK);
                case HttpStatusCode.NotFound:
                    return (loteReturn, HttpStatusCode.NotFound);
                case HttpStatusCode.InternalServerError:
                    return (loteReturn, HttpStatusCode.InternalServerError);
            }
            return (loteReturn, HttpStatusCode.NotFound);
        }

        public async Task<(List<ApiPessoaCadastro>, HttpStatusCode)> GetPessoas()
        {
            List<ApiPessoaCadastro> pessoaReturn = null;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var builder = new UriBuilder(_configuration["pessoaServiceEndpoint"]);
            string url = builder.ToString();
            var result = await client.GetAsync(url);
            switch (result.StatusCode)
            {
                case HttpStatusCode.OK:
                    pessoaReturn = JsonConvert.DeserializeObject<List<ApiPessoaCadastro>>(await client.GetStringAsync(url));
                    return (pessoaReturn, HttpStatusCode.OK);
                case HttpStatusCode.NotFound:
                    return (pessoaReturn, HttpStatusCode.NotFound);
                case HttpStatusCode.InternalServerError:
                    return (pessoaReturn, HttpStatusCode.InternalServerError);
            }
            return (pessoaReturn, HttpStatusCode.NotFound);
        }
    }
}