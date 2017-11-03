using Agrobook.Domain.Usuarios;
using Eventing;
using Eventing.Core.Persistence;
using System.Threading.Tasks;

namespace Agrobook.Domain.Ap
{
    /// <summary>
    /// El id provider. Una buena tecnica, como se puede ver con el numero de servicio, es que se prefije, y solamente alli.
    /// </summary>
    public static class ApIdProvider
    {
        public async static Task ValidarIdProductor(string idUsuario, IEventSourcedReader reader)
        {
            await reader.EnsureExistenceOfThis<Usuario>(idUsuario);
        }

        public async static Task ValidarIdDeNumeracionDeServiciosPorProductor(string idProductor, IEventSourcedReader reader)
        {
            await ValidarIdProductor(idProductor, reader);
        }

        public async static Task<string> ObtenerNuevoIdDeServicio(int ultimoNumeroDeServicioDelProductor, string idProductor, IEventSourcedReader reader)
        {
            await ValidarIdProductor(idProductor, reader);
            // prod: por lo del productor
            // sn: servicio numero...
            var numero = ultimoNumeroDeServicioDelProductor + 1;
            var idServicio = $"{idProductor}-sn{numero}";
            return idServicio;
        }

        public async static Task<string> ObtenerNuevoIdContrato(string idOrganizacion, string nombreCrudoDelContrato, IEventSourcedReader reader)
        {
            await reader.EnsureExistenceOfThis<Organizacion>(idOrganizacion);

            Ensure.NotNullOrWhiteSpace(nombreCrudoDelContrato, nameof(nombreCrudoDelContrato));

            var nombreFormateado = nombreCrudoDelContrato.ToTrimmedAndWhiteSpaceless();

            return $"{idOrganizacion}-{nombreFormateado}";
        }

        public async static Task<string> ObtenerNuevoIdAdenda(string idContrato, string nombreAdendaCruda, IEventSourcedReader reader)
        {
            await reader.EnsureExistenceOfThis<Contrato>(idContrato);

            Ensure.NotNullOrWhiteSpace(nombreAdendaCruda, nameof(nombreAdendaCruda));

            return $"{idContrato}-{nombreAdendaCruda.ToTrimmedAndWhiteSpaceless()}";
        }
    }
}
