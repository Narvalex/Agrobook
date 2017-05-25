namespace Agrobook.Domain.Usuarios.Login
{
    public static class ClaimsDefs
    {
        public static class Roles
        {
            public const string Admin = "rol-admin";
            public const string Gerente = "rol-gerente";
            public const string Tecnico = "rol-tecnico";
            public const string Productor = "rol-productor";
            public const string Invitado = "rol-invitado";
        }

        public static class Permisos
        {
            public const string AdministrarOrganizaciones = "permiso-administrar-organizaciones";
        }
    }
}
