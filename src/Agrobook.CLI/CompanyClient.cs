using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Agrobook.CLI
{
    public class CompanyClient
    {
        private readonly Func<HttpClient> clientFactory;

        public CompanyClient(string hostUri)
        {
            this.clientFactory = () =>
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(new Uri(hostUri), "api/companies/");
                return client;
            };
        }

        public IEnumerable<Company> GetCompanies()
        {
            HttpResponseMessage response;
            using (var client = this.clientFactory.Invoke())
            {
                response = client.GetAsync(client.BaseAddress).Result;
            }
            var result = response.Content.ReadAsAsync<IEnumerable<Company>>().Result;
            return result;
        }

        public Company GetCompany(int id)
        {
            HttpResponseMessage response;
            using (var client = this.clientFactory.Invoke())
            {
                response = client.GetAsync(
                    new Uri(client.BaseAddress, id.ToString())).Result;
            }
            var result = response.Content.ReadAsAsync<Company>().Result;
            return result;
        }


        public System.Net.HttpStatusCode AddCompany(Company company)
        {
            HttpResponseMessage response;
            using (var client = this.clientFactory.Invoke())
            {
                response = client.PostAsJsonAsync(client.BaseAddress, company).Result;
            }
            return response.StatusCode;
        }


        public System.Net.HttpStatusCode UpdateCompany(Company company)
        {
            HttpResponseMessage response;
            using (var client = this.clientFactory.Invoke())
            {
                response = client.PutAsJsonAsync(client.BaseAddress, company).Result;
            }
            return response.StatusCode;
        }


        public System.Net.HttpStatusCode DeleteCompany(int id)
        {
            HttpResponseMessage response;
            using (var client = this.clientFactory.Invoke())
            {
                response = client.DeleteAsync(
                    new Uri(client.BaseAddress, id.ToString())).Result;
            }
            return response.StatusCode;
        }
    }

    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
