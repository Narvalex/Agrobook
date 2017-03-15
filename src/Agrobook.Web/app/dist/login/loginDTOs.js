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
        function loginResult(loginExitoso, nombreParaMostrar, token) {
            this.loginExitoso = loginExitoso;
            this.nombreParaMostrar = nombreParaMostrar;
            this.token = token;
        }
        return loginResult;
    }());
    login.loginResult = loginResult;
})(login || (login = {}));
//# sourceMappingURL=loginDTOs.js.map