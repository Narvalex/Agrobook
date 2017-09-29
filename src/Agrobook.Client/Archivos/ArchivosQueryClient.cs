using Agrobook.Domain.Archivos.Services;
using Eventing.Client.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Agrobook.Client.Archivos
{
    public class ArchivosQueryClient : ClientBase
    {
        public ArchivosQueryClient(HttpLite http, Func<string> tokenProvider = null)
            : base(http, tokenProvider, "archivos/query")
        { }

        public async Task<Stream> Download(string idColeccion, string nombreArchivo, string usuario)
        {
            string prefix, sufix;
            prefix = "download";
            sufix = $"/{usuario}";
            var stream = await base.Get($"{prefix}/{idColeccion}/{nombreArchivo}" + sufix);
            return stream;
        }

        public async Task<Stream> Preview(string idColeccion, string nombreArchivo, string usuario)
        {
            string prefix, sufix;
            prefix = "preview";
            sufix = $"/{usuario}";
            var stream = await base.Get($"{prefix}/{idColeccion}/{nombreArchivo}" + sufix);
            return stream;
        }

        public async Task<IList<MetadatosDeArchivo>> ObtenerListaDeArchivos(string idColeccion)
        {
            var lista = await base.Get<IList<MetadatosDeArchivo>>("coleccion/" + idColeccion);
            return lista;
        }
    }
}