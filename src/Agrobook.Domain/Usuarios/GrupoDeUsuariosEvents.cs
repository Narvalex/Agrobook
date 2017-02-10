using Agrobook.Domain.Common;

namespace Agrobook.Domain.Usuarios
{
    public class NuevoGrupoCreado : MensajeAuditable
    {
        public NuevoGrupoCreado(Metadatos metadatos, string idGrupo)
            : base(metadatos)
        {
            this.IdGrupo = idGrupo;
        }

        public string IdGrupo { get; }
    }

    public class UsuarioAgregadoAGrupo : MensajeAuditable
    {
        public UsuarioAgregadoAGrupo(Metadatos metadatos, string idGrupo, string idUsuario)
            : base(metadatos)
        {
            this.IdGrupo = idGrupo;
            this.IdUsuario = idUsuario;
        }

        public string IdGrupo { get; }
        public string IdUsuario { get; }
    }
}
