using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ApiSharedMed
{
    public class ConexionSupIntSalud
    {
        private const string urlApi = "https://api.superdesalud.gob.cl/prestadores/v1";
  

        public static HttpResponseMessage GetApi(string route, string urlParameters = "")
        {

            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri(urlApi + route);
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = client.GetAsync(urlParameters).Result;

            return response;

        }
    }
}
