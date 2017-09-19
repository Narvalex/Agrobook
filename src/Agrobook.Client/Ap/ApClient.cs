using Agrobook.Domain.Ap.Messages;
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

        /// <summary>
        /// Registra un nueva adenda
        /// </summary>
        /// <param name="cmd">El comando</param>
        /// <returns>El id de la adenda</returns>
        public async Task<string> Send(RegistrarNuevaAdenda cmd)
            => await base.Post<RegistrarNuevaAdenda, string>("registrar-adenda", cmd);


        public async Task Send(EditarAdenda cmd)
            => await base.Post("editar-adenda", cmd);
    }
}
