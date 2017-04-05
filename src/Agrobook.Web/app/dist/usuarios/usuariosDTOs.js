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
    var usuarioEnLista = (function () {
        function usuarioEnLista(nombre, avatarUrl) {
            this.nombre = nombre;
            this.avatarUrl = avatarUrl;
        }
        return usuarioEnLista;
    }());
    usuariosArea.usuarioEnLista = usuarioEnLista;
})(usuariosArea || (usuariosArea = {}));
//# sourceMappingURL=usuariosDTOs.js.map