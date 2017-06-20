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
    var claimDto = (function () {
        function claimDto(id, display, info) {
            this.id = id;
            this.display = display;
            this.info = info;
        }
        return claimDto;
    }());
    usuariosArea.claimDto = claimDto;
    var actualizarPerfilDto = (function () {
        function actualizarPerfilDto(usuario, avatarUrl, nombreParaMostrar, passwordActual, nuevoPassword) {
            this.usuario = usuario;
            this.avatarUrl = avatarUrl;
            this.nombreParaMostrar = nombreParaMostrar;
            this.passwordActual = passwordActual;
            this.nuevoPassword = nuevoPassword;
        }
        return actualizarPerfilDto;
    }());
    usuariosArea.actualizarPerfilDto = actualizarPerfilDto;
    var organizacionDto = (function () {
        function organizacionDto(id, display, usuarioEsMiembro) {
            this.id = id;
            this.display = display;
            this.usuarioEsMiembro = usuarioEsMiembro;
        }
        return organizacionDto;
    }());
    usuariosArea.organizacionDto = organizacionDto;
    var grupoDto = (function () {
        function grupoDto(id, display) {
            this.id = id;
            this.display = display;
        }
        return grupoDto;
    }());
    usuariosArea.grupoDto = grupoDto;
})(usuariosArea || (usuariosArea = {}));
//# sourceMappingURL=usuariosDTOs.js.map