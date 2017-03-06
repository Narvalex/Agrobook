/// <reference path="../_all.ts" />
var UsuariosArea;
(function (UsuariosArea) {
    var UsuarioDto = (function () {
        function UsuarioDto(avatarUrl, nombreDeUsuario, password) {
            this.avatarUrl = avatarUrl;
            this.nombreDeUsuario = nombreDeUsuario;
            this.password = password;
        }
        return UsuarioDto;
    }());
    UsuariosArea.UsuarioDto = UsuarioDto;
})(UsuariosArea || (UsuariosArea = {}));
//# sourceMappingURL=usuariosDTOs.js.map