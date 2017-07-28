using System;

namespace Agrobook.Domain.Common
{
    public class MensajeAuditable
    {
        public MensajeAuditable(Firma firma)
        {
            this.Firma = firma;
        }

        public Firma Firma { get; private set; }

        public bool TrySet(Firma firma)
        {
            // Only updates if metadatos is null
            if (this.Firma == null)
            {
                this.Firma = firma;
                return true;
            }

            return false;
        }
    }

    public interface IProveedorDeFirmaDelUsuario
    {
        Firma ObtenerFirmaDelUsuario(string token);
    }

    public class Firma
    {
        public Firma(string usuario, DateTime timestamp)
        {
            this.Usuario = usuario;
            this.Timestamp = timestamp;
        }

        public string Usuario { get; }
        public DateTime Timestamp { get; }
    }
}
