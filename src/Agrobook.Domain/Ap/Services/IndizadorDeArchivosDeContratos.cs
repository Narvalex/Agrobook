using Agrobook.Domain.Archivos;
using Agrobook.Domain.Archivos.Subscribers;
using System.Linq;

namespace Agrobook.Domain.Ap
{
    public class IndizadorDeArchivosDeContratos : IIndizadorDeAreaEspecifica
    {
        public AgrobookDbContext AgregarAlIndice(AgrobookDbContext context, string idColeccion)
            => Procesar(context, idColeccion, true);

        public AgrobookDbContext EliminarDelIndice(AgrobookDbContext context, string idColeccion)
            => Procesar(context, idColeccion, false);

        public AgrobookDbContext RestaurarEnElIndice(AgrobookDbContext context, string idColeccion)
            => Procesar(context, idColeccion, true);

        private AgrobookDbContext Procesar(AgrobookDbContext context, string idColeccion, bool tieneArchivo)
        {
            var descriptor = this.Parsear(idColeccion);
            if (!descriptor.EsArchivoDeContrato) return context;

            var entity = context.Contratos.Single(x => x.Id == descriptor.IdContrato);

            entity.TieneArchivo = tieneArchivo;

            return context;
        }

        private DescriptorDeArchivoDeContrato Parsear(string idColeccion)
        {
            var tokens = idColeccion.Split('-');
            if (tokens.First() != ColeccionDeArchivosIdProvider.orgContratos)
                return new DescriptorDeArchivoDeContrato { EsArchivoDeContrato = false };

            return new DescriptorDeArchivoDeContrato
            {
                EsArchivoDeContrato = true,
                IdContrato = idColeccion.Substring(idColeccion.IndexOf('-') + 1)
            };
        }

        private class DescriptorDeArchivoDeContrato
        {
            public bool EsArchivoDeContrato { get; set; }
            public string IdContrato { get; set; }
        }
    }
}
