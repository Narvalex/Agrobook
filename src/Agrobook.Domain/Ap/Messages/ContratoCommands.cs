using Agrobook.Domain.Common;
using System;

namespace Agrobook.Domain.Ap.Messages
{
    public class RegistrarNuevoContrato : MensajeAuditable
    {
        public RegistrarNuevoContrato(Firma firma, string idOrganizacion, string nombreDelContrato, DateTime fecha)
            : base(firma)
        {
            this.IdOrganizacion = idOrganizacion;
            this.NombreDelContrato = nombreDelContrato;
            this.Fecha = fecha;
        }

        public string IdOrganizacion { get; }
        public string NombreDelContrato { get; }
        public DateTime Fecha { get; }
    }

    public class EditarContrato : MensajeAuditable
    {
        public EditarContrato(Firma firma, string idContrato, string nombreDelContrato, DateTime fecha)
            : base(firma)
        {
            this.IdContrato = idContrato;
            this.NombreDelContrato = nombreDelContrato;
            this.Fecha = fecha;
        }

        public string IdContrato { get; }
        public string NombreDelContrato { get; }
        public DateTime Fecha { get; }
    }

    public class EliminarContrato : MensajeAuditable
    {
        public EliminarContrato(Firma firma, string id) : base(firma)
        {
            this.Id = id;
        }

        public string Id { get; }
    }

    public class RestaurarContrato : MensajeAuditable
    {
        public RestaurarContrato(Firma firma, string id) : base(firma)
        {
            this.Id = id;
        }

        public string Id { get; }
    }

    public class RegistrarNuevaAdenda : MensajeAuditable
    {
        public RegistrarNuevaAdenda(Firma firma, string idContrato, string nombreDeLaAdenda, DateTime fecha)
            : base(firma)
        {
            this.IdContrato = idContrato;
            this.NombreDeLaAdenda = nombreDeLaAdenda;
            this.Fecha = fecha;
        }

        public string IdContrato { get; }
        public string NombreDeLaAdenda { get; }
        public DateTime Fecha { get; }
    }

    public class EditarAdenda : MensajeAuditable
    {
        public EditarAdenda(Firma firma, string idContrato, string idAdenda, string nombreDeLaAdenda, DateTime fecha)
            : base(firma)
        {
            this.IdContrato = idContrato;
            this.IdAdenda = idAdenda;
            this.NombreDeLaAdenda = nombreDeLaAdenda;
            this.Fecha = fecha;
        }

        public string IdContrato { get; }
        public string IdAdenda { get; }
        public string NombreDeLaAdenda { get; }
        public DateTime Fecha { get; }
    }

    public class EliminarAdenda : MensajeAuditable
    {
        public EliminarAdenda(Firma firma, string idContrato, string idAdenda) : base(firma)
        {
            this.IdContrato = idContrato;
            this.IdAdenda = idAdenda;
        }

        public string IdContrato { get; }
        public string IdAdenda { get; }
    }

    public class RestaurarAdenda : MensajeAuditable
    {
        public RestaurarAdenda(Firma firma, string idContrato, string idAdenda) : base(firma)
        {
            this.IdContrato = idContrato;
            this.IdAdenda = idAdenda;
        }

        public string IdContrato { get; }
        public string IdAdenda { get; }
    }
}
