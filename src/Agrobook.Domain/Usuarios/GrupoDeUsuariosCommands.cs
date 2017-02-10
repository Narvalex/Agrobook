using Agrobook.Domain.Common;

namespace Agrobook.Domain.Usuarios
{
    public class CrearGrupo : MensajeAuditable
    {
        public CrearGrupo(string idGrupo, Metadatos metadatos)
            : base(metadatos)
        {
            this.IdGrupo = idGrupo;
        }

        public string IdGrupo { get; }
    }

    public class AgregarUsuarioAGrupo : MensajeAuditable
    {
        public AgregarUsuarioAGrupo(string idGrupo, string idUsuario, Metadatos metadatos)
            : base(metadatos)
        {
            this.IdGrupo = idGrupo;
            this.IdUsuario = idUsuario;
        }

        public string IdGrupo { get; }
        public string IdUsuario { get; }
    }
}
