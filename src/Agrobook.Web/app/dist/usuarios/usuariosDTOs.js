/// <reference path="../_all.ts" />
var usuariosArea;
(function (usuariosArea) {
    var UsuarioDto = (function () {
        function UsuarioDto(avatarUrl, nombreDeUsuario, nombreParaMostrar, password) {
            this.avatarUrl = avatarUrl;
            this.nombreDeUsuario = nombreDeUsuario;
            this.nombreParaMostrar = nombreParaMostrar;
            this.password = password;
        }
        return UsuarioDto;
    }());
    usuariosArea.UsuarioDto = UsuarioDto;
})(usuariosArea || (usuariosArea = {}));
//# sourceMappingURL=usuariosDTOs.js.map