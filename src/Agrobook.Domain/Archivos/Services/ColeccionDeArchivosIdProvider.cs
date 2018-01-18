using System;
using System.Collections.Generic;
using System.Linq;

namespace Agrobook.Domain.Archivos
{
    public static class ColeccionDeArchivosIdProvider
    {
        private static readonly HashSet<string> prefijos;

        public const string orgContratos = "orgContratos";

        static ColeccionDeArchivosIdProvider()
        {
            prefijos = ResolverPrefijos();
        }

        public static void ValidarElIdDeColecionPropuesto(string idColeccion)
        {
            if (idColeccion.Contains(' '))
                throw new ArgumentException("La id colección no debe contener espacios en blanco");

            var tokens = idColeccion.Split('-');

            if (!prefijos.Contains(tokens.First()))
                throw new ArgumentException($"El prefijo {tokens.First()} no es válido para ser pertenecer a un id colección.");
        }

        private static HashSet<string> ResolverPrefijos()
            => new HashSet<string>
            {
                "servicioDatosBasicos",     // seria el de la página resumen y el del Informe final
                "servicioParcelas",         // parcelas del servicio
                "servicioDiagnostico",      // diagnostico del servicio
                "servicioPrescripciones",   // prescripciones del servicio
                orgContratos              // contratos con la organizacion: Contrato, Adenda I, Adenda II etc.
            };
    }
}
