/// <reference path="../_all.ts" />
var archivosArea;
(function (archivosArea) {
    var sidenavController = (function () {
        function sidenavController($mdSidenav, usuariosQueryService, toasterLite, $scope, $rootScope, config, $mdDialog, loginService) {
            var _this = this;
            this.$mdSidenav = $mdSidenav;
            this.usuariosQueryService = usuariosQueryService;
            this.toasterLite = toasterLite;
            this.$scope = $scope;
            this.$rootScope = $rootScope;
            this.config = config;
            this.$mdDialog = $mdDialog;
            this.loginService = loginService;
            // Auth
            this.puedeCargarArchivos = false;
            this.eventoCarga = null;
            this.filtroSeleccionado = this.config.tiposDeArchivos.todos;
            // Auth
            var roles = this.config.claims.roles;
            this.puedeCargarArchivos = this.loginService.autorizar([roles.Gerente, roles.Tecnico]);
            this.$scope.$on(this.config.eventIndex.archivos.productorSeleccionado, function (e, args) {
                _this.idProductor = args;
                _this.recuperarInfoDelProductor();
            });
            this.$scope.$on(this.config.eventIndex.archivos.abrirCuadroDeCargaDeArchivos, function (e, args) {
                _this.idProductor = args;
                _this.recuperarInfoDelProductor();
                _this.initializeUploadCenter();
            });
            var tipos = this.config.tiposDeArchivos;
            this.filtros = [
                tipos.todos,
                tipos.pdf,
                tipos.mapas,
                tipos.fotos,
                tipos.excel,
                tipos.word,
                tipos.powerPoint
            ];
        }
        sidenavController.prototype.toggleSideNav = function () {
            this.$mdSidenav('left').toggle();
        };
        sidenavController.prototype.mostrarDialogoDeCarga = function ($event) {
            this.eventoCarga = $event;
            if (location.hash.slice(3, 9) === 'upload')
                this.initializeUploadCenter();
            else
                window.location.replace('#!/upload/' + this.idProductor);
        };
        sidenavController.prototype.filtrar = function (filtro) {
            this.filtroSeleccionado = filtro;
            this.$rootScope.$broadcast(this.config.eventIndex.archivos.filtrar, filtro);
        };
        // INTERNAL
        sidenavController.prototype.initializeUploadCenter = function () {
            //this.toasterLite.info(`nuevo archivo para ${this.idProductor}!`);
            this.$mdDialog.show({
                templateUrl: '../app/dist/archivos/dialogs/upload-center-dialog.html',
                parent: angular.element(document.body),
                targetEvent: this.eventoCarga,
                clickOutsideToClose: true,
                fullscreen: true
            })
                .then(function (answer) {
                console.log('Modal aceptado...');
            }, function () {
            });
        };
        sidenavController.prototype.recuperarInfoDelProductor = function () {
            var _this = this;
            this.usuariosQueryService.obtenerInfoBasicaDeUsuario(this.idProductor, function (value) {
                _this.productor = new archivosArea.productorDto(value.data.nombre, value.data.nombreParaMostrar, value.data.avatarUrl, null);
            }, function (reason) {
                _this.toasterLite.error('Ocurrió un error al recuperar información del usuario', _this.toasterLite.delayForever);
            });
        };
        return sidenavController;
    }());
    sidenavController.$inject = ['$mdSidenav', 'usuariosQueryService', 'toasterLite', '$scope', '$rootScope', 'config', '$mdDialog',
        'loginService'
    ];
    archivosArea.sidenavController = sidenavController;
})(archivosArea || (archivosArea = {}));
//# sourceMappingURL=sidenavController.js.map