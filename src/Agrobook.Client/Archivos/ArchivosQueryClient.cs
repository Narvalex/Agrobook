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

        public async Task<IList<ProductorDto>> ObtenerProductores()
        {
            var lista = await base.Get<IList<ProductorDto>>("productores");
            return lista;
        }

        public async Task<IList<ArchivoDto>> ObtenerArchivosDelProductor(string idProductor)
        {
            var lista = await base.Get<IList<ArchivoDto>>("archivos-del-productor/" + idProductor);
            return lista;
        }

        public async Task<Stream> Download(string idProductor, string nombreArchivo, string extension, string usuario = null)
        {
            string prefix, sufix;
            if (usuario is null)
            {
                prefix = "preview";
                sufix = string.Empty;
            }
            else
            {
                prefix = "download";
                sufix = $"/{usuario}";
            }
            var stream = await base.Get($"{prefix}/{idProductor}/{nombreArchivo}/{extension}" + sufix);
            return stream;
        }
    }
}