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
        function loginResult() {
        }
        return loginResult;
    }());
    login.loginResult = loginResult;
})(login || (login = {}));
//# sourceMappingURL=loginDTOs.js.map