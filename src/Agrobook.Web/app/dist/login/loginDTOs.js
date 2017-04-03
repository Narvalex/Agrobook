/// <reference path="../_all.ts" />
var login;
(function (login) {
    var credencialesDto = (function () {
        function credencialesDto(usuario, password) {
            this.usuario = usuario;
            this.password = password;
        }
        return credencialesDto;
    }());
    login.credencialesDto = credencialesDto;
    var loginResult = (function () {
        function loginResult(loginExitoso, nombreParaMostrar, token, avatarUrl) {
            this.loginExitoso = loginExitoso;
            this.nombreParaMostrar = nombreParaMostrar;
            this.token = token;
            this.avatarUrl = avatarUrl;
        }
        return loginResult;
    }());
    login.loginResult = loginResult;
})(login || (login = {}));
//# sourceMappingURL=loginDTOs.js.map