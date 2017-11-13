using Agrobook.Domain.Ap.Commands;
using Eventing.Client.Http;
using System;
using System.Threading.Tasks;

namespace Agrobook.Client.Ap
{
    public class ApClient : ClientBase
    {
        public ApClient(HttpLite http, Func<string> tokenProvider = null)
            : base(http, tokenProvider, "ap")
        { }

        /// <summary>
        /// Registra un nuevo contrato
        /// </summary>
        /// <param name="cmd">El comando</param>
        /// <returns>El id del nuevo contrato</returns>
        public async Task<string> Send(RegistrarNuevoContrato cmd)
            => await base.Post<RegistrarNuevoContrato, string>("registrar-contrato", cmd);

        public async Task Send(EditarContrato cmd)
            => await base.Post("editar-contrato", cmd);

        public async Task EliminarContrato(string id)
            => await base.Post("eliminar-contrato/" + id, "");

        public async Task RestaurarContrato(string id)
        {
            await base.Post("restaurar-contrato/" + id, "");
        }

        /// <summary>
        /// Registra un nueva adenda
        /// </summary>
        /// <param name="cmd">El comando</param>
        /// <returns>El id de la adenda</returns>
        public async Task<string> Send(RegistrarNuevaAdenda cmd)
            => await base.Post<RegistrarNuevaAdenda, string>("registrar-adenda", cmd);

        public async Task Send(EditarAdenda cmd)
            => await base.Post("editar-adenda", cmd);

        public async Task EliminarAdenda(string idContrato, string idAdenda)
            => await base.Post("eliminar-adenda?idContrato=" + idContrato + "&idAdenda=" + idAdenda, "");

        public async Task RestaurarAdenda(string idContrato, string idAdenda)
        {
            await base.Post("restaurar-adenda?idContrato=" + idContrato + "&idAdenda=" + idAdenda, "");
        }

        public async Task<string> Send(RegistrarParcela cmd)
        {
            var idParcela = await base.Post<RegistrarParcela, string>("registrar-parcela", cmd);
            return idParcela;
        }

        public async Task Send(EditarParcela cmd)
            => await base.Post("editar-parcela", cmd);

        public async Task Send(EliminarParcela cmd)
            => await base.Post("eliminar-parcela", cmd);

        public async Task Send(RestaurarParcela cmd)
            => await base.Post("restaurar-parcela", cmd);

        /// <summary>
        /// Registra nuevo servicio de agricultura de precisión.
        /// </summary>
        /// <param name="cmd">El comando que para ordenar el registro.</param>
        /// <returns>El id del nuevo servicio registrado</returns>
        public async Task<string> Send(RegistrarNuevoServicio cmd)
            => await base.Post<RegistrarNuevoServicio, string>("nuevo-servicio", cmd);

        public async Task Send(EditarDatosBasicosDelSevicio cmd)
            => await base.Post("editar-datos-basicos-del-servicio", cmd);

        public async Task Send(EliminarServicio cmd)
            => await base.Post("eliminar-servicio", cmd);

        public async Task Send(RestaurarServicio cmd)
            => await base.Post("restaurar-servicio", cmd);

        public async Task Send(EspecificarParcelaDelServicio cmd)
            => await base.Post("especificar-parcela-del-servicio", cmd);

        public async Task Send(CambiarParcelaDelServicio cmd)
            => await base.Post("cambiar-parcela-del-servicio", cmd);

        public async Task Send(FijarPrecioAlServicio cmd) => await base.Post("fijar-precio-al-servicio", cmd);

        public async Task Send(AjustarPrecioDelServicio cmd) => await base.Post("ajustar-precio-del-servicio", cmd);
    }
}
