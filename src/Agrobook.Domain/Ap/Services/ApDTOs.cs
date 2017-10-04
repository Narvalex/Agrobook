using System;

namespace Agrobook.Domain.Ap.Services
{
    public class ClienteDeApDto
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string Desc { get; set; }
        public string Tipo { get; set; }
        public string AvatarUrl { get; set; }
    }

    public class OrgDto
    {
        public string Id { get; set; }
        public string Display { get; set; }
        public string AvatarUrl { get; set; }
    }

    public class OrgConContratosDto
    {
        public OrgDto Org { get; set; }
        public ContratoEntity[] Contratos { get; set; }
    }

    public class ProdDto
    {
        public string Id { get; set; }
        public string Display { get; set; }
        public string AvatarUrl { get; set; }
        public OrgDto[] Orgs { get; set; }
    }

    public class ServicioDto
    {
        public string Id { get; set; }
        public string IdContrato { get; set; }
        public bool EsAdenda { get; set; }
        public string IdContratoDeLaAdenda { get; set; }
        public string ContratoDisplay { get; set; }
        public string IdOrg { get; set; }
        public string OrgDisplay { get; set; }
        public string IdProd { get; set; }
        public DateTime Fecha { get; set; }

        // Defaults
        public bool Eliminado { get; set; }
        public string ParcelaId { get; set; }
        public string ParcelaDisplay { get; set; }
    }
}
