using Agrobook.Domain.Ap.Services;
using System.Web.Http;

namespace Agrobook.Server.Ap
{
    [RoutePrefix("ap")]
    public partial class ApController : ApiController
    {
        private readonly ApService service = ServiceLocator.ResolveSingleton<ApService>();
        private readonly NumeradorDeServiciosCommandHandler numeradorDeServicios = ServiceLocator.ResolveSingleton<NumeradorDeServiciosCommandHandler>();
    }
}
