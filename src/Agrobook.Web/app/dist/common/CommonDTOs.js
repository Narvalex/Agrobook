/// <reference path="../_all.ts" />
var common;
(function (common) {
    var perfilActualizado = (function () {
        function perfilActualizado(usuario, avatarUrl, nombreParaMostrar) {
            this.usuario = usuario;
            this.avatarUrl = avatarUrl;
            this.nombreParaMostrar = nombreParaMostrar;
        }
        return perfilActualizado;
    }());
    common.perfilActualizado = perfilActualizado;
})(common || (common = {}));
//# sourceMappingURL=CommonDTOs.js.map