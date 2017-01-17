using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Agrobook.Server
{
    public class CompaniesController : ApiController
    {
        private static List<Company> _db = new List<Company>
        {
            new Company { Id = 1, Name = "Microsoft" },
            new Company { Id = 2, Name = "Google" },
            new Company { Id = 3, Name = "Apple" }
        };

        public async Task<IEnumerable<Company>> Get() => await Task.Run(() => _db);

        public async Task<Company> Get(int id)
        {
            var company = await Task.Run(() => _db.FirstOrDefault(c => c.Id == id));
            if (company == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return company;
        }

        public async Task<IHttpActionResult> Post(Company company)
        {
            if (company == null)
            {
                return this.BadRequest("Argument Null");
            }
            var companyExists = await Task.Run(() => _db.Any(c => c.Id == company.Id));

            if (companyExists) return this.BadRequest("Exists");

            _db.Add(company);
            return this.Ok();
        }

        public async Task<IHttpActionResult> Put(Company company)
        {
            if (company == null)
                return this.BadRequest("Argument null");

            var existing = await Task.Run(() => _db.FirstOrDefault(x => x.Id == company.Id));

            if (existing == null) return this.NotFound();

            existing.Name = company.Name;
            return this.Ok();
        }

        public async Task<IHttpActionResult> Delete(int id)
        {
            var company = await Task.Run(() => _db.FirstOrDefault(c => c.Id == id));
            if (company == null) return this.Ok();

            _db.Remove(company);
            return this.Ok();
        }
    }

    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
