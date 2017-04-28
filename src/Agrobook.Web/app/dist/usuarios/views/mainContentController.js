/// <reference path="../../_all.ts" />
var usuariosArea;
(function (usuariosArea) {
    var mainContentController = (function () {
        function mainContentController($routeParams, loginQueryService, usuariosQueryService, toasterLite, $scope, config) {
            var _this = this;
            this.$routeParams = $routeParams;
            this.loginQueryService = loginQueryService;
            this.usuariosQueryService = usuariosQueryService;
            this.toasterLite = toasterLite;
            this.$scope = $scope;
            this.config = config;
            this.loaded = false;
            var idUsuario = this.$routeParams['idUsuario'];
            if (idUsuario === undefined)
                idUsuario = this.loginQueryService.tryGetLocalLoginInfo().usuario;
            this.usuariosQueryService.obtenerInfoBasicaDeUsuario(idUsuario, function (value) {
                _this.usuario = value.data;
                _this.loaded = true;
            }, function (reason) {
                _this.toasterLite.error('Ocurrió un error al recuperar información del usuario ' + idUsuario, _this.toasterLite.delayForever);
            });
            this.$scope.$on(this.config.eventIndex.usuarios.perfilActualizado, function (e, args) {
                _this.usuario = new usuariosArea.usuarioInfoBasica(_this.usuario.nombre, args.nombreParaMostrar, args.avatarUrl);
            });
        }
        return mainContentController;
    }());
    mainContentController.$inject = ['$routeParams', 'loginQueryService', 'usuariosQueryService', 'toasterLite', '$scope',
        'config'];
    usuariosArea.mainContentController = mainContentController;
})(usuariosArea || (usuariosArea = {}));
//# sourceMappingURL=mainContentController.js.map