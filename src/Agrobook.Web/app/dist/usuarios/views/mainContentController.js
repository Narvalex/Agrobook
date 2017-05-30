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
            this.abrirElTabQueCorrespondeDesdeUrl();
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
            this.$scope.$on('$routeUpdate', function (scope, next, current) {
                _this.abrirElTabQueCorrespondeDesdeUrl();
            });
        }
        mainContentController.prototype.onTabSelect = function (tabIndex) {
            if (this.usuario === undefined)
                return;
            var tabId;
            switch (tabIndex) {
                case 0:
                    tabId = 'perfil';
                    break;
                case 1:
                    tabId = 'permisos';
                    break;
                case 2:
                    tabId = 'grupos';
                    break;
                case 3:
                    tabId = 'organizaciones';
                    break;
                default: tabId = "perfil";
            }
            window.location.replace('#!/usuario/' + this.usuario.nombre + '?tab=' + tabId);
        };
        mainContentController.prototype.abrirElTabQueCorrespondeDesdeUrl = function () {
            var tabId = this.$routeParams['tab'];
            switch (tabId) {
                case 'perfil':
                    this.tabIndex = 0;
                    break;
                case 'permisos':
                    this.tabIndex = 1;
                    break;
                case 'grupos':
                    this.tabIndex = 2;
                    break;
                case 'organizaciones':
                    this.tabIndex = 3;
                    break;
                default:
                    this.tabIndex = 0;
                    break;
            }
        };
        return mainContentController;
    }());
    mainContentController.$inject = ['$routeParams', 'loginQueryService', 'usuariosQueryService', 'toasterLite', '$scope',
        'config'];
    usuariosArea.mainContentController = mainContentController;
})(usuariosArea || (usuariosArea = {}));
//# sourceMappingURL=mainContentController.js.map