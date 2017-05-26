using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Agrobook.Domain.Usuarios.Login.ClaimsDefs;

namespace Agrobook.Domain.Usuarios.Login
{
    public static class ClaimProvider
    {
        private static Dictionary<string, Claim> _claims =
            new Dictionary<string, Claim>
            {
                // Roles
                { Roles.Admin, new Claim(Roles.Admin, "Admin", "Los administradores tienen el control total del sistema.") },
                { Roles.Gerente, new Claim(Roles.Gerente, "Gerente", "Los gerentes administran casi todo el sistema.") },
                { Roles.Tecnico, new Claim(Roles.Tecnico, "Técnico", "Los técnicos realizan trabajos para los productores.") },
                { Roles.Productor, new Claim(Roles.Productor, "Productor", "Los productores son los clientes.") },
                { Roles.Invitado, new Claim(Roles.Invitado, "Invitado", "Los invitados pueden ver los trabajos realizados a productores de su organización.") },
                // Permisos
                { Permisos.AdministrarOrganizaciones, new Claim(Permisos.AdministrarOrganizaciones, "Permiso para administrar organizaciones", "Los que tengan este permiso pueden administrar las organizaciones del sistema") }
            };

        public static int ClaimCount => _claims.Count();

        public static IDictionary<string, Claim> Todos => _claims;

        public static Claim[] ObtenerClaimsPermitidosParaCrearNuevoUsuario(string claim)
        {
            switch (claim)
            {
                case Roles.Admin:
                    return _claims.Values.ToArray();

                case Roles.Gerente:
                    return _claims.Values.SkipWhile(c => c.Id == Roles.Admin || c.Id == Roles.Gerente).ToArray();

                case Roles.Tecnico:
                    return new Claim[] { _claims[Roles.Productor] };

                default:
                    return null;
            }
        }

        public static Claim[] ObtenerClaimsPermitidosParaCrearNuevoUsuario(string[] claims)
        {
            var list = new List<Claim>();
            for (int i = 0; i < claims.Length; i++)
            {
                var parcial = ObtenerClaimsPermitidosParaCrearNuevoUsuario(claims[i]);

                if (parcial == null) continue;

                for (int j = 0; j < parcial.Length; j++)
                {
                    if (!list.Any(x => x.Id == parcial[j].Id))
                        list.Add(parcial[j]);
                }
            }

            if (list.Count == 0) return null;

            return list.ToArray();
        }
    }
}