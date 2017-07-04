using Agrobook.Domain.Archivos.Services;
using System;
using System.Collections.Generic;
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
    }
}