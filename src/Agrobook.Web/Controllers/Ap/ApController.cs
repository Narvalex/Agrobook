﻿using Agrobook.Domain.Ap.Services;
using System.Web.Http;

namespace Agrobook.Web.Controllers.Ap
{
    [RoutePrefix("app/ap")]
    public class ApController : ApiControllerBase
    {
        private readonly ApService service = ServiceLocator.ResolveSingleton<ApService>();

        public ApController()
        {

        }
    }
}