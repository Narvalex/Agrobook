/// <reference path="../_all.ts" />
var usuariosArea;
(function (usuariosArea) {
    var usuarioDto = /** @class */ (function () {
        function usuarioDto(avatarUrl, nombreDeUsuario, nombreParaMostrar, password, telefono, email, claims) {
            this.avatarUrl = avatarUrl;
            this.nombreDeUsuario = nombreDeUsuario;
            this.nombreParaMostrar = nombreParaMostrar;
            this.password = password;
            this.telefono = telefono;
            this.email = email;
            this.claims = claims;
        }
        return usuarioDto;
    }());
    usuariosArea.usuarioDto = usuarioDto;
    var usuarioInfoBasica = /** @class */ (function () {
        function usuarioInfoBasica(nombre, nombreParaMostrar, avatarUrl, telefono, email) {
            this.nombre = nombre;
            this.nombreParaMostrar = nombreParaMostrar;
            this.avatarUrl = avatarUrl;
            this.telefono = telefono;
            this.email = email;
        }
        return usuarioInfoBasica;
    }());
    usuariosArea.usuarioInfoBasica = usuarioInfoBasica;
    var claimDto = /** @class */ (function () {
        function claimDto(id, display, info) {
            this.id = id;
            this.display = display;
            this.info = info;
        }
        return claimDto;
    }());
    usuariosArea.claimDto = claimDto;
    var actualizarPerfilDto = /** @class */ (function () {
        function actualizarPerfilDto(usuario, avatarUrl, nombreParaMostrar, passwordActual, nuevoPassword, telefono, email) {
            this.usuario = usuario;
            this.avatarUrl = avatarUrl;
            this.nombreParaMostrar = nombreParaMostrar;
            this.passwordActual = passwordActual;
            this.nuevoPassword = nuevoPassword;
            this.telefono = telefono;
            this.email = email;
        }
        return actualizarPerfilDto;
    }());
    usuariosArea.actualizarPerfilDto = actualizarPerfilDto;
    var organizacionDto = /** @class */ (function () {
        function organizacionDto(id, display, usuarioEsMiembro, deleted) {
            if (deleted === void 0) { deleted = false; }
            this.id = id;
            this.display = display;
            this.usuarioEsMiembro = usuarioEsMiembro;
            this.deleted = deleted;
        }
        return organizacionDto;
    }());
    usuariosArea.organizacionDto = organizacionDto;
    var grupoDto = /** @class */ (function () {
        function grupoDto(id, display, usuarioEsMiembro) {
            this.id = id;
            this.display = display;
            this.usuarioEsMiembro = usuarioEsMiembro;
        }
        return grupoDto;
    }());
    usuariosArea.grupoDto = grupoDto;
})(usuariosArea || (usuariosArea = {}));
//# sourceMappingURL=usuariosDTOs.js.map