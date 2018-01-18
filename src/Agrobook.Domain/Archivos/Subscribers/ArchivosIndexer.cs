using Agrobook.Domain.Archivos.Subscribers;
using Agrobook.Domain.Common;
using Eventing;
using Eventing.Core.Messaging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Agrobook.Domain.Archivos.Services
{
    public class ArchivosIndexer : AgrobookSqlDenormalizer,
        IHandler<NuevoArchivoAgregadoALaColeccion>,
        IHandler<ArchivoEliminado>,
        IHandler<ArchivoRestaurado>
    {
        private readonly IFileWriter fileManager;
        private readonly List<IIndizadorDeAreaEspecifica> indizadores = new List<IIndizadorDeAreaEspecifica>();

        public ArchivosIndexer(AgrobookSqlDenormalizerConfig config,
            IFileWriter fileManager, params IIndizadorDeAreaEspecifica[] indizadores)
            : base(config)
        {
            Ensure.NotNull(fileManager, nameof(fileManager));

            this.fileManager = fileManager;
            this.indizadores.AddRange(indizadores);
        }

        public async Task Handle(long eventNumber, NuevoArchivoAgregadoALaColeccion e)
        {
            var renombrado = this.fileManager.SetFileAsIndexedIfNeeded(e.IdColeccion, e.Descriptor);

            this.Denormalize(eventNumber, context =>
            {
                context.Archivos.Add(new ArchivosEntity
                {
                    IdColeccion = e.IdColeccion,
                    Nombre = e.Descriptor.Nombre,
                    Fecha = e.Descriptor.Fecha,
                    Extension = e.Descriptor.Extension,
                    Tipo = e.Descriptor.Tipo,
                    Size = e.Descriptor.Size,
                    Eliminado = false
                });

                this.indizadores.ForEach(i => i.AgregarAlIndice(context, e.IdColeccion));
            });
        }

        public async Task Handle(long eventNumber, ArchivoEliminado e)
        {
            this.Denormalize(eventNumber, context =>
            {
                var archivo = context.Archivos.Single(x => x.IdColeccion == e.IdColeccion && x.Nombre == e.NombreArchivo);
                archivo.Eliminado = true;

                this.indizadores.ForEach(i => i.EliminarDelIndice(context, e.IdColeccion));
            });
        }

        public async Task Handle(long eventNumber, ArchivoRestaurado e)
        {
            this.Denormalize(eventNumber, context =>
            {
                var archivo = context.Archivos.Single(x => x.IdColeccion == e.IdColeccion && x.Nombre == e.NombreArchivo);
                archivo.Eliminado = false;

                this.indizadores.ForEach(i => i.EliminarDelIndice(context, e.IdColeccion));
            });
        }
    }
}
