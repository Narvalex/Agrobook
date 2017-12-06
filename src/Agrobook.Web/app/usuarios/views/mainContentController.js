/// <reference path="../../_all.ts" />
var usuariosArea;
(function (usuariosArea) {
    var mainContentController = /** @class */ (function () {
        function mainContentController($routeParams, loginService, loginQueryService, usuariosQueryService, toasterLite, $scope, config) {
            var _this = this;
            this.$routeParams = $routeParams;
            this.loginService = loginService;
            this.loginQueryService = loginQueryService;
            this.usuariosQueryService = usuariosQueryService;
            this.toasterLite = toasterLite;
            this.$scope = $scope;
            this.config = config;
            this.loaded = false;
            this.mostrarOrganizacionesYGrupos = false;
            // auth
            var claims = this.config.claims;
            this.mostrarOrganizacionesYGrupos = this.loginService.autorizar([
                claims.roles.Admin,
                claims.roles.Gerente,
                claims.roles.Tecnico
            ]);
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
                _this.usuario = new usuariosArea.usuarioInfoBasica(_this.usuario.nombre, args.nombreParaMostrar, args.avatarUrl, args.telefono, args.email);
            });
            this.abrirTabCorrespondiente();
            this.$scope.$on('$routeUpdate', function (scope, next, current) {
                _this.abrirTabCorrespondiente();
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
                    tabId = 'grupos';
                    break;
                case 2:
                    tabId = 'organizaciones';
                    break;
                default: tabId = "perfil";
            }
            window.location.replace('#!/usuario/' + this.usuario.nombre + '?tab=' + tabId);
        };
        mainContentController.prototype.abrirTabCorrespondiente = function () {
            var tabId = this.$routeParams['tab'];
            switch (tabId) {
                case 'perfil':
                    this.tabIndex = 0;
                    break;
                case 'grupos':
                    this.tabIndex = 1;
                    break;
                case 'organizaciones':
                    this.tabIndex = 2;
                    break;
                default:
                    this.tabIndex = 0;
                    break;
            }
        };
        mainContentController.$inject = ['$routeParams', 'loginService', 'loginQueryService', 'usuariosQueryService', 'toasterLite', '$scope',
            'config'];
        return mainContentController;
    }());
    usuariosArea.mainContentController = mainContentController;
})(usuariosArea || (usuariosArea = {}));
//# sourceMappingURL=mainContentController.js.map