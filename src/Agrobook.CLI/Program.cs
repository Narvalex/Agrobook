using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Agrobook.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            // Wait for the async stuff to run...
            Run().Wait();

            // Then Write Done...
            Console.WriteLine("");
            Console.WriteLine("Done! Press the Enter key to Exit...");
            Console.ReadLine();
            return;
        }

        static async Task Run()
        {
            // Create an http client provider:
            var hostUriString = "http://localhost:8080";
            var provider = new ApiClientProvider(hostUriString);
            string accessToken;
            Dictionary<string, string> tokenDictionary = null;

            bool errorOccurred = false;
            try
            {
                tokenDictionary = await provider.GetTokenDictionary("sampleuser", "ssword");
                accessToken = tokenDictionary["access_token"];
            }
            catch (AggregateException ex)
            {
                // If it's an aggregate exception, an async error occurred;
                Console.WriteLine(ex.InnerExceptions[0].Message);
                errorOccurred = true;
            }
            catch (Exception ex)
            {
                // Something else happened:
                Console.WriteLine(ex.Message);
                errorOccurred = true;
            }

            if (!errorOccurred)
            {
                // Write the contents of the dictionary
                foreach (var kvp in tokenDictionary)
                {
                    Console.WriteLine($"{kvp.Key}: {kvp.Value}");
                    Console.WriteLine(string.Empty);
                }
            }
        }


        static void OldMain(string[] args)
        {
            Console.WriteLine("Read all the companies...");
            var companyClient = new CompanyClient("http://localhost:8080");
            var companies = companyClient.GetCompanies();
            WriteCompaniesList(companies);

            int nextId = (from c in companies select c.Id).Max() + 1;

            Console.WriteLine("Add a new company...");
            var result = companyClient.AddCompany(
                new Company
                {
                    Id = nextId,
                    Name = string.Format("New Company #{0}", nextId)
                });
            WriteStatusCodeResult(result);

            Console.WriteLine("Updated List after Add:");
            companies = companyClient.GetCompanies();
            WriteCompaniesList(companies);

            Console.WriteLine("Update a company...");
            var updateMe = companyClient.GetCompany(nextId);
            updateMe.Name = string.Format("Updated company #{0}", updateMe.Id);
            result = companyClient.UpdateCompany(updateMe);
            WriteStatusCodeResult(result);

            Console.WriteLine("Updated List after Update:");
            companies = companyClient.GetCompanies();
            WriteCompaniesList(companies);

            Console.WriteLine("Delete a company...");
            result = companyClient.DeleteCompany(nextId - 1);
            WriteStatusCodeResult(result);

            Console.WriteLine("Updated List after Delete:");
            companies = companyClient.GetCompanies();
            WriteCompaniesList(companies);

            Console.Read();
        }


        static void WriteCompaniesList(IEnumerable<Company> companies)
        {
            foreach (var company in companies)
            {
                Console.WriteLine("Id: {0} Name: {1}", company.Id, company.Name);
            }
            Console.WriteLine("");
        }


        static void WriteStatusCodeResult(System.Net.HttpStatusCode statusCode)
        {
            if (statusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine("Opreation Succeeded - status code {0}", statusCode);
            }
            else
            {
                Console.WriteLine("Opreation Failed - status code {0}", statusCode);
            }
            Console.WriteLine("");
        }
    }
}
