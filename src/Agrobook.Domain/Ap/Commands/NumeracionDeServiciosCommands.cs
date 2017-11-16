using Agrobook.Domain.Common;
using System;

namespace Agrobook.Domain.Ap.Commands
{
    public class RegistrarNuevoServicio : MensajeAuditable
    {
        public RegistrarNuevoServicio(Firma firma, string idProd, string idOrg, string idContrato, bool esAdenda, string idContratoDeLaAdenda, DateTime fecha,
            string observaciones)
            : base(firma)
        {
            this.IdProd = idProd;
            this.IdOrg = idOrg;
            this.IdContrato = idContrato;
            this.EsAdenda = esAdenda;
            this.IdContratoDeLaAdenda = idContratoDeLaAdenda;
            this.Fecha = fecha;
            this.Observaciones = observaciones;
        }

        public string IdProd { get; }
        public string IdOrg { get; }
        public string IdContrato { get; }
        public bool EsAdenda { get; }
        public string IdContratoDeLaAdenda { get; }
        public DateTime Fecha { get; }
        public string Observaciones { get; }
    }
}
