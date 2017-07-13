/// <reference path="../_all.ts" />
var archivosArea;
(function (archivosArea) {
    var sidenavController = (function () {
        function sidenavController($mdSidenav, usuariosQueryService, toasterLite, $rootScope, config, $mdDialog, loginService) {
            var _this = this;
            this.$mdSidenav = $mdSidenav;
            this.usuariosQueryService = usuariosQueryService;
            this.toasterLite = toasterLite;
            this.$rootScope = $rootScope;
            this.config = config;
            this.$mdDialog = $mdDialog;
            this.loginService = loginService;
            // Auth
            this.puedeCargarArchivos = false;
            this.eventoCarga = null;
            // Auth
            var roles = this.config.claims.roles;
            this.puedeCargarArchivos = this.loginService.autorizar([roles.Gerente, roles.Tecnico]);
            this.$rootScope.$on(this.config.eventIndex.archivos.productorSeleccionado, function (e, args) {
                _this.idProductor = args;
                _this.recuperarInfoDelProductor();
            });
            this.$rootScope.$on(this.config.eventIndex.archivos.abrirCuadroDeCargaDeArchivos, function (e, args) {
                _this.idProductor = args;
                _this.recuperarInfoDelProductor();
                _this.initializeUploadCenter();
            });
            this.filtros = [
                new FiltroDto("Todos", "list"),
                new FiltroDto("Fotos", "picture"),
                new FiltroDto("PDF", "pdf"),
                new FiltroDto("Mapas", "google-earth"),
                new FiltroDto("Excel", "excel"),
                new FiltroDto("Word", "word"),
                new FiltroDto("PowerPoint", "powerPoint")
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
    sidenavController.$inject = ['$mdSidenav', 'usuariosQueryService', 'toasterLite', '$rootScope', 'config', '$mdDialog',
        'loginService'
    ];
    archivosArea.sidenavController = sidenavController;
    var FiltroDto = (function () {
        function FiltroDto(display, icon) {
            this.display = display;
            this.icon = icon;
        }
        return FiltroDto;
    }());
    archivosArea.FiltroDto = FiltroDto;
})(archivosArea || (archivosArea = {}));
//# sourceMappingURL=sidenavController.js.map