/// <reference path="../_all.ts" />
var usuariosArea;
(function (usuariosArea) {
    var UsuarioDto = (function () {
        function UsuarioDto(avatarUrl, nombreDeUsuario, nombreParaMostrar, password, claims) {
            this.avatarUrl = avatarUrl;
            this.nombreDeUsuario = nombreDeUsuario;
            this.nombreParaMostrar = nombreParaMostrar;
            this.password = password;
            this.claims = claims;
        }
        return UsuarioDto;
    }());
    usuariosArea.UsuarioDto = UsuarioDto;
    var usuarioInfoBasica = (function () {
        function usuarioInfoBasica(nombre, nombreCompleto, avatarUrl) {
            this.nombre = nombre;
            this.nombreCompleto = nombreCompleto;
            this.avatarUrl = avatarUrl;
        }
        return usuarioInfoBasica;
    }());
    usuariosArea.usuarioInfoBasica = usuarioInfoBasica;
})(usuariosArea || (usuariosArea = {}));
//# sourceMappingURL=usuariosDTOs.js.map