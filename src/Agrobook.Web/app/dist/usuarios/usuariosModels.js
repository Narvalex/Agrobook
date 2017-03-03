/// <reference path="../_all.ts" />
var UsuariosArea;
(function (UsuariosArea) {
    var Usuario = (function () {
        function Usuario(avatarUrl, nombreDeUsuario, password) {
            this.avatarUrl = avatarUrl;
            this.nombreDeUsuario = nombreDeUsuario;
            this.password = password;
        }
        return Usuario;
    }());
    UsuariosArea.Usuario = Usuario;
})(UsuariosArea || (UsuariosArea = {}));
//# sourceMappingURL=usuariosModels.js.map