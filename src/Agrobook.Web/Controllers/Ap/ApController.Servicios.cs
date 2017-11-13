using Agrobook.Domain.Ap.Commands;
using System.Threading.Tasks;
using System.Web.Http;

namespace Agrobook.Web.Controllers.Ap
{
    public partial class ApController
    {
        [HttpPost]
        [Route("nuevo-servicio")]
        public async Task<IHttpActionResult> NuevoServicio([FromBody]RegistrarNuevoServicio cmd)
        {
            var idServicio = await this.client.Send(cmd);
            return this.Ok(idServicio);
        }

        [HttpPost]
        [Route("editar-datos-basicos-del-servicio")]
        public async Task<IHttpActionResult> EditarDatosBasicosDelServicio([FromBody]EditarDatosBasicosDelSevicio cmd)
        {
            await this.client.Send(cmd);
            return this.Ok();
        }

        [HttpPost]
        [Route("eliminar-servicio")]
        public async Task<IHttpActionResult> EliminarServicio([FromBody]EliminarServicio cmd)
        {
            await this.client.Send(cmd);
            return this.Ok();
        }

        [HttpPost]
        [Route("restaurar-servicio")]
        public async Task<IHttpActionResult> RestaurarServicio([FromBody]RestaurarServicio cmd)
        {
            await this.client.Send(cmd);
            return this.Ok();
        }

        [HttpPost]
        [Route("especificar-parcela-del-servicio")]
        public async Task<IHttpActionResult> EspecificarParcelaDelServicio([FromBody]EspecificarParcelaDelServicio cmd)
        {
            await this.client.Send(cmd);
            return this.Ok();
        }

        [HttpPost]
        [Route("cambiar-parcela-del-servicio")]
        public async Task<IHttpActionResult> CambiarParcelaDelServicio([FromBody]CambiarParcelaDelServicio cmd)
        {
            await this.client.Send(cmd);
            return this.Ok();
        }

        [HttpPost]
        [Route("fijar-precio-al-servicio")]
        public async Task<IHttpActionResult> FijarPrecio([FromBody]FijarPrecioAlServicio cmd)
        {
            await this.client.Send(cmd);
            return this.Ok();
        }

        [HttpPost]
        [Route("ajustar-precio-del-servicio")]
        public async Task<IHttpActionResult> FijarPrecio([FromBody]AjustarPrecioDelServicio cmd)
        {
            await this.client.Send(cmd);
            return this.Ok();
        }
    }
}