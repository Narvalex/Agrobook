using Agrobook.Core;
using Agrobook.Domain.Common;
using Eventing;
using Eventing.Core.Domain;
using Eventing.Core.Persistence;
using System;
using System.Threading.Tasks;

namespace Agrobook.Domain.Archivos.Services
{
    public class ArchivosIndexerService : AgrobookDenormalizer,
        IEventHandler<NuevoArchivoAgregadoALaColeccion>
    {
        private readonly IArchivosDelProductorFileManager fileManager;

        public ArchivosIndexerService(IEventSubscriber subscriber, Func<AgrobookDbContext> contextFactory,
            IArchivosDelProductorFileManager fileManager)
            : base(subscriber, contextFactory,
                  typeof(ArchivosIndexerService).Name,
                  StreamCategoryAttribute.GetCategoryProjectionStream<ColeccionDeArchivosDelProductor>())
        {
            Ensure.NotNull(fileManager, nameof(fileManager));

            this.fileManager = fileManager;
        }

        public async Task Handle(long eventNumber, NuevoArchivoAgregadoALaColeccion e)
        {
            var renombrado = this.fileManager.SetFileAsIndexedIfNeeded(e.IdProductor, e.Archivo);

            await this.Denormalize(eventNumber, context =>
            {
                context.Archivos.Add(new ArchivosEntity
                {
                    IdProductor = e.IdProductor,
                    Nombre = e.Archivo.Nombre,
                    Fecha = e.Archivo.Fecha,
                    Extension = e.Archivo.Extension,
                    Descripcion = e.Archivo.Descripcion,
                    Size = e.Archivo.Size
                });
            });
        }
    }
}
