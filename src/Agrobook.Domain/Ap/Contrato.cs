using Eventing.Core.Domain;

namespace Agrobook.Domain.Ap
{
    /// <summary>
    /// Un contrato de servicios profesionales de Agricultura de Precisión con una organización.
    /// Al contrato se le puede aplicar varias adendas, que extienden los términos del contrato.
    /// </summary>
    [StreamCategory("agrobook.contratos")]
    public class Contrato : EventSourced
    {
        public Contrato()
        {
            // this.On...
        }
    }
}
