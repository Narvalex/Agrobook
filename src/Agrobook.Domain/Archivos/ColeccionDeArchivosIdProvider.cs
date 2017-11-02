using System;
using System.Linq;

namespace Agrobook.Domain.Archivos
{
    public static class ColeccionDeArchivosIdProvider
    {
        /// <summary>
        /// Retorna el id de la coleccion validada. Puede ser cualquier identificador sin espacios.
        /// Por ahora se hace desde el cliente, en el typescript. Por ejemplo: {seccion}-{servicio}-{idProductor}.
        /// Cada vista es responsable de su id. Esto no parece ser muy bueno, pero es lo que hay...
        /// </summary>
        /// <remarks>
        /// Se puede consultar en la directiva filesWidgetDirective.ts. Cada seccion del cliente tiene su lógica.
        /// Por ejemplo: los mapas de limites sería algo así como: mapaDeLimites-servicio1-productor2. 
        /// Para una migración, esto se debe tener en cuenta.
        /// </remarks>
        public static string ValidarElIdDeColecionPropuesto(string idColeccionPropuesta)
        {
            if (idColeccionPropuesta.Contains(' '))
                throw new ArgumentException("La id colección no debe contener espacios en blanco");

            return idColeccionPropuesta;
        }
    }
}
