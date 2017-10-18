using System.Collections.Generic;
using System.Linq;
using static Agrobook.Domain.Usuarios.Login.ClaimDef;

namespace Agrobook.Domain.Usuarios.Login
{
    public static class ClaimProvider
    {
        private static Dictionary<string, Claim> _claims =
            new Dictionary<string, Claim>
            {
                // Roles
                { Roles.Admin, new Claim(Roles.Admin, TipoDeClaim.Rol, "Admin", "Los administradores tienen el control total del sistema.") },
                { Roles.Gerente, new Claim(Roles.Gerente, TipoDeClaim.Rol, "Gerente", "Los gerentes administran casi todo el sistema.") },
                { Roles.Tecnico, new Claim(Roles.Tecnico, TipoDeClaim.Rol, "Técnico", "Los técnicos realizan trabajos para los productores.") },
                { Roles.Productor, new Claim(Roles.Productor, TipoDeClaim.Rol, "Productor", "Los productores son los clientes.") },
                { Roles.Invitado, new Claim(Roles.Invitado, TipoDeClaim.Rol, "Invitado", "Los invitados pueden ver los trabajos realizados a productores de su organización.") },
                // Permisos
                //{ Permisos.AdministrarOrganizaciones, new Claim(Permisos.AdministrarOrganizaciones, TipoDeClaim.Permiso, "Permiso para administrar organizaciones", "Los que tengan este permiso pueden administrar las organizaciones del sistema") }
            };

        public static int ClaimCount => _claims.Count();

        public static IDictionary<string, Claim> Todos => _claims;

        public static IEnumerable<Claim> ObtenerClaimsValidos(string[] claims)
        {
            return claims.Select(x => _claims[x]);
        }

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