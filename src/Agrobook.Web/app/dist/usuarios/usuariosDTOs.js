/// <reference path="../_all.ts" />
var usuariosArea;
(function (usuariosArea) {
    var usuarioDto = (function () {
        function usuarioDto(avatarUrl, nombreDeUsuario, nombreParaMostrar, password, claims) {
            this.avatarUrl = avatarUrl;
            this.nombreDeUsuario = nombreDeUsuario;
            this.nombreParaMostrar = nombreParaMostrar;
            this.password = password;
            this.claims = claims;
        }
        return usuarioDto;
    }());
    usuariosArea.usuarioDto = usuarioDto;
    var usuarioInfoBasica = (function () {
        function usuarioInfoBasica(nombre, nombreParaMostrar, avatarUrl) {
            this.nombre = nombre;
            this.nombreParaMostrar = nombreParaMostrar;
            this.avatarUrl = avatarUrl;
        }
        return usuarioInfoBasica;
    }());
    usuariosArea.usuarioInfoBasica = usuarioInfoBasica;
    var actualizarPerfilDto = (function () {
        function actualizarPerfilDto(avatarUrl, nombreParaMostrar, passwordActual, nuevoPassword) {
            this.avatarUrl = avatarUrl;
            this.nombreParaMostrar = nombreParaMostrar;
            this.passwordActual = passwordActual;
            this.nuevoPassword = nuevoPassword;
        }
        return actualizarPerfilDto;
    }());
    usuariosArea.actualizarPerfilDto = actualizarPerfilDto;
})(usuariosArea || (usuariosArea = {}));
//# sourceMappingURL=usuariosDTOs.js.map