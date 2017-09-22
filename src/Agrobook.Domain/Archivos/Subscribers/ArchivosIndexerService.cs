using Agrobook.Common;
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
        private readonly IFileWriter fileManager;

        public ArchivosIndexerService(IEventSubscriber subscriber, Func<AgrobookDbContext> contextFactory,
            IFileWriter fileManager)
            : base(subscriber, contextFactory,
                  typeof(ArchivosIndexerService).Name,
                  StreamCategoryAttribute.GetCategoryProjectionStream<ColeccionDeArchivos>())
        {
            Ensure.NotNull(fileManager, nameof(fileManager));

            this.fileManager = fileManager;
        }

        public async Task HandleOnce(long eventNumber, NuevoArchivoAgregadoALaColeccion e)
        {
            var renombrado = this.fileManager.SetFileAsIndexedIfNeeded(e.IdColeccion, e.Descriptor);

            await this.Denormalize(eventNumber, context =>
            {
                context.Archivos.Add(new ArchivosEntity
                {
                    IdColeccion = e.IdColeccion,
                    Nombre = e.Descriptor.Nombre,
                    Fecha = e.Descriptor.Fecha,
                    Extension = e.Descriptor.Extension,
                    Tipo = e.Descriptor.Tipo,
                    Size = e.Descriptor.Size
                });
            });
        }
    }
}
