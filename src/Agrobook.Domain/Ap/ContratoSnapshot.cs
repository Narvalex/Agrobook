using Eventing.Core.Domain;
using System.Collections.Generic;

namespace Agrobook.Domain.Ap
{
    public class ContratoSnapshot : Snapshot
    {
        public ContratoSnapshot(string streamName, int version, string idOrganizacion, KeyValuePair<string, bool>[] adendas,
            bool estaEliminado)
            : base(streamName, version)
        {
            this.IdOrganizacion = idOrganizacion;
            this.Adendas = adendas;
            this.EstaEliminado = estaEliminado;
        }

        public string IdOrganizacion { get; }
        public KeyValuePair<string, bool>[] Adendas { get; }
        public bool EstaEliminado { get; }
    }
}
