using System;

namespace Agrobook.Domain.Common
{
    public class MensajeAuditable
    {
        public MensajeAuditable(Metadatos metadatos)
        {
            this.Metadatos = metadatos;
        }

        public Metadatos Metadatos { get; }
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
