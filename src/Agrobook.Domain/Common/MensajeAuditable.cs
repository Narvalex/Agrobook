using System;

namespace Agrobook.Domain.Common
{
    public class MensajeAuditable
    {
        public MensajeAuditable(Metadatos metadatos)
        {
            this.Metadatos = metadatos;
        }

        public Metadatos Metadatos { get; private set; }

        public bool TrySet(Metadatos metadatos)
        {
            // Only updates if metadatos is null
            if (this.Metadatos == null)
            {
                this.Metadatos = metadatos;
                return true;
            }

            return false;
        }
    }

    public class Metadatos
    {
        public Metadatos(string autor, DateTime timestamp)
        {
            this.Autor = autor;
            this.Timestamp = timestamp;
        }

        public string Autor { get; }
        public DateTime Timestamp { get; }
    }
}
