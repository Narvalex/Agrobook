/// <reference path="../_all.ts" />
var login;
(function (login) {
    var credencialesDto = /** @class */ (function () {
        function credencialesDto(usuario, password) {
            this.usuario = usuario;
            this.password = password;
        }
        return credencialesDto;
    }());
    login.credencialesDto = credencialesDto;
    var loginResult = /** @class */ (function () {
        function loginResult(loginExitoso, usuario, nombreParaMostrar, token, avatarUrl, claims) {
            this.loginExitoso = loginExitoso;
            this.usuario = usuario;
            this.nombreParaMostrar = nombreParaMostrar;
            this.token = token;
            this.avatarUrl = avatarUrl;
            this.claims = claims;
        }
        return loginResult;
    }());
    login.loginResult = loginResult;
})(login || (login = {}));
//# sourceMappingURL=loginDTOs.js.map