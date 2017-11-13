using Agrobook.Client.Usuarios;
using System;
using static System.Console;

namespace Agrobook.CLI.Controllers
{
    public class SeedView
    {
        internal void ShowWellcomeScreen()
        {
            WriteLine("Bienvenido al subsistema Seed de Agrobook");
        }

        internal string PedirIniciarOCancelarSeed()
        {
            Console.WriteLine("Por favor ingrese la letra [S] si quiere iniciar o presione otra tecla para cancelar");
            return Console.ReadLine();
        }

        internal void MostrarQueElProcesoSeedSeInicio()
        {
            Console.WriteLine("Se inició el proceso del Seed, por favor espere...");
        }

        internal void MostrarQueElProcesoSeedFinalizo()
        {
            Console.WriteLine("El proceso del Seed finalizó correctamente");
        }

        internal void MostrarQueSeSaleDelSeed()
        {
            Console.WriteLine("Se canceló el seeding de Agrobook");
        }

        internal void NotificarUsuarioCreado(UsuarioDto dto)
        {
            Console.WriteLine("Se creó el usuario " + dto.NombreDeUsuario);
        }

        internal void NotificarOrgCreada(string nombreDeLaOrg, string id)
        {
            Console.WriteLine("Se creo la org " + nombreDeLaOrg + " con el id: " + id);
        }

        internal void NotificarUsuarioAgregadoAOrg(string usuario, string org)
        {
            Console.WriteLine($"Se agrego a '{usuario}' a la org '{org}'");
        }

        internal void NotificarNuevoContrato(string contrato, string org)
        {
            Console.WriteLine($"Nuevo contrato '{contrato}' con la org '{org}'");
        }

        internal void NotificarNuevaAdenda(string adenda, string org)
        {
            Console.WriteLine($"Nueva adenda '{adenda}' con la org '{org}'");
        }
    }
}
