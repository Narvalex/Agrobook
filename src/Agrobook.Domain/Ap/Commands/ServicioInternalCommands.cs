namespace Agrobook.Domain.Ap.Commands
{
    internal class ProcesarRegistroDeServicioPendiente
    {
        internal ProcesarRegistroDeServicioPendiente(NuevoRegistroDeServicioPendiente registroPendiente)
        {
            this.RegistroPendiente = registroPendiente;
        }

        internal NuevoRegistroDeServicioPendiente RegistroPendiente { get; }
    }
}
