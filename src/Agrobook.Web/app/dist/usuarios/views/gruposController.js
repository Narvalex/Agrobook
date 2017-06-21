/// <reference path="../../_all.ts" />
var usuariosArea;
(function (usuariosArea) {
    var gruposController = (function () {
        function gruposController(usuariosService, usuariosQueryService, loginQueryService, toasterLite, $mdDialog, $timeout, $q, $log, $rootScope, $routeParams, config) {
            var _this = this;
            this.usuariosService = usuariosService;
            this.usuariosQueryService = usuariosQueryService;
            this.loginQueryService = loginQueryService;
            this.toasterLite = toasterLite;
            this.$mdDialog = $mdDialog;
            this.$timeout = $timeout;
            this.$q = $q;
            this.$log = $log;
            this.$rootScope = $rootScope;
            this.$routeParams = $routeParams;
            this.config = config;
            // loading org
            this.loaded = false;
            // loading grupos for an org
            this.gruposLoaded = true;
            this.creandoGrupo = false;
            this.filterFromServer = false;
            this.isDisabled = false;
            // list of `organizaciones` value/display objects
            this.organizaciones = [];
            // list of grupos
            this.grupos = [];
            this.idUsuario = this.$routeParams['idUsuario'];
            if (this.idUsuario === undefined)
                this.idUsuario = this.loginQueryService.tryGetLocalLoginInfo().usuario;
            this.recuperarListaDeOrganizaciones();
            this.$rootScope.gruposController = {};
            this.$rootScope.$on(this.config.eventIndex.usuarios.usuarioAgregadoAOrganizacion, function (e, args) {
                if (_this.idUsuario === args.idUsuario) {
                    // angular le da una propiedad nueva al objeto que parece que le corrompe. Por eso creo uno  nuevo.
                    var dto = new usuariosArea.organizacionDto(args.org.id, args.org.display, false);
                    _this.organizaciones.push(dto);
                }
            });
        }
        // ******************************
        // Public methods
        // ******************************
        gruposController.prototype.crearOrganizacion = function () {
            var idUsuario = this.loginQueryService.tryGetLocalLoginInfo().usuario;
            window.location.replace('#!/usuario/' + idUsuario + '?tab=organizaciones');
        };
        gruposController.prototype.crearNuevoGrupo = function ($event) {
            var _this = this;
            this.creandoGrupo = true;
            this.$rootScope.gruposController.orgSeleccionada = this.orgSeleccionada;
            this.$mdDialog.show({
                templateUrl: '../app/dist/usuarios/dialogs/nuevo-grupo-dialog.html',
                parent: angular.element(document.body),
                targetEvent: $event,
                controller: usuariosArea.nuevoGrupoDialogController,
                controllerAs: 'vm',
                clickOutsideToClose: true
            }).then(function (nuevoGrupo) {
                // Agregar nuevo grupo a la lista, si fue exitosa
                _this.creandoGrupo = false;
            }, function () {
                _this.toasterLite.info('Creación de grupo cancelada');
                _this.creandoGrupo = false;
            });
        };
        //********************************
        // Internal
        //********************************
        // ******************************
        // Autocomplete stuff
        // ******************************
        gruposController.prototype.recuperarListaDeOrganizaciones = function () {
            var _this = this;
            this.usuariosQueryService.obtenerOrganizacionesDelUsuario(this.idUsuario, function (response) {
                var lista = [];
                for (var i = 0; i < response.data.length; i++) {
                    lista.push(new usuariosArea.organizacionDto(response.data[i].id, response.data[i].display, response.data[i].usuarioEsMiembro));
                }
                _this.organizaciones = lista;
                _this.loaded = true;
            }, function (reason) { return _this.toasterLite.error('Hubo un error al recuperar lista de organizaciones', _this.toasterLite.delayForever); });
        };
        gruposController.prototype.searchTextChange = function (text) {
            //this.$log.info('Text changed to ' + text);
        };
        gruposController.prototype.selectedItemChange = function (org) {
            var _this = this;
            if (org === undefined) {
                // dejo en blanco el filtro
                this.grupos = [];
                return;
            }
            this.gruposLoaded = false;
            this.usuariosQueryService.obtenerGrupos(org.id, function (value) {
                _this.gruposLoaded = true;
                _this.grupos = value.data;
            }, function (reason) { _this.toasterLite.error('Error al cargar grupos', _this.toasterLite.delayForever); });
        };
        gruposController.prototype.refreshOrgList = function () {
            return this.organizaciones;
        };
        return gruposController;
    }());
    gruposController.$inject = ['usuariosService', 'usuariosQueryService', 'loginQueryService', 'toasterLite',
        '$mdDialog', '$timeout', '$q', '$log', '$rootScope', '$routeParams', 'config'];
    usuariosArea.gruposController = gruposController;
})(usuariosArea || (usuariosArea = {}));
//# sourceMappingURL=gruposController.js.map