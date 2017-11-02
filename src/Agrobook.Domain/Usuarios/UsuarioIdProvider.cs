using System;
using System.Linq;

namespace Agrobook.Domain.Usuarios.Services
{
    public static class UsuarioIdProvider
    {
        /// <summary>
        /// El id del usuario sera el que escriba el creador el usaurio. El mismo debe ser 
        /// sin espacios. Ejemplo: jgimenez.
        /// </summary>
        /// <param name="nombreDeUsuario">El nombre de usuario ingresado.</param>
        /// <returns>El id validado</returns>
        public static string ValidarElNombreDeUsuario(string nombreDeUsuario)
        {
            if (nombreDeUsuario.Contains(' '))
                throw new ArgumentException("El nombre de usuario no debe contener espacios en blanco");
            return nombreDeUsuario;
        }

        /// <summary>
        /// Retorna un id de organizacion a travéz del nombre designado que puede contener espacios en 
        /// blanco y otras cosas. Ejemplo: CooperativaChortizerLtda.
        /// </summary>
        /// <param name="nombreOrganizacionEnCrudo">El nombre de la organización tal cual se escribió.</param>
        /// <returns>El id validado y sin espacios en blanco.</returns>
        public static string ObtenerIdOrganizacionAPartirDelNombre(string nombreOrganizacionEnCrudo)
        {
            var nombreFormateadoParaDisplay = nombreOrganizacionEnCrudo.Trim();
            var nombreFormateadoParaId = nombreOrganizacionEnCrudo.ToTrimmedAndWhiteSpaceless().ToLowerInvariant();
            return nombreFormateadoParaId;
        }
    }
}
