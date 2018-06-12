/// <reference path="../_all.ts" />
var common;
(function (common) {
    var perfilActualizado = (function () {
        function perfilActualizado(usuario, avatarUrl, nombreParaMostrar, telefono, email) {
            this.usuario = usuario;
            this.avatarUrl = avatarUrl;
            this.nombreParaMostrar = nombreParaMostrar;
            this.telefono = telefono;
            this.email = email;
        }
        return perfilActualizado;
    }());
    common.perfilActualizado = perfilActualizado;
})(common || (common = {}));
//# sourceMappingURL=CommonDTOs.js.map