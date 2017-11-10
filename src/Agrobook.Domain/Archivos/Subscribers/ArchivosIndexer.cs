using Agrobook.Domain.Common;
using Eventing;
using Eventing.Core.Messaging;
using System.Linq;

namespace Agrobook.Domain.Archivos.Services
{
    public class ArchivosIndexer : SqlDenormalizer,
        IHandler<NuevoArchivoAgregadoALaColeccion>,
        IHandler<ArchivoEliminado>,
        IHandler<ArchivoRestaurado>
    {
        private readonly IFileWriter fileManager;

        public ArchivosIndexer(SqlDenormalizerConfig config,
            IFileWriter fileManager)
            : base(config)
        {
            Ensure.NotNull(fileManager, nameof(fileManager));

            this.fileManager = fileManager;
        }

        public void Handle(long eventNumber, NuevoArchivoAgregadoALaColeccion e)
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
            });
        }

        public void Handle(long eventNumber, ArchivoEliminado e)
        {
            this.Denormalize(eventNumber, context =>
            {
                var archivo = context.Archivos.Single(x => x.IdColeccion == e.IdColeccion && x.Nombre == e.NombreArchivo);
                archivo.Eliminado = true;
            });
        }

        public void Handle(long eventNumber, ArchivoRestaurado e)
        {
            this.Denormalize(eventNumber, context =>
            {
                var archivo = context.Archivos.Single(x => x.IdColeccion == e.IdColeccion && x.Nombre == e.NombreArchivo);
                archivo.Eliminado = false;
            });
        }
    }
}
