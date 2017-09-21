using Eventing.Core.Domain;
using System.Collections.Generic;

namespace Agrobook.Domain.Ap
{
    public class ProductorSnapshot : Snapshot
    {
        public ProductorSnapshot(string streamName, int version, KeyValuePair<string, bool>[] parcelas) : base(streamName, version)
        {
            this.Parcelas = parcelas;
        }

        public KeyValuePair<string, bool>[] Parcelas { get; }
    }
}
