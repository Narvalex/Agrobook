/// <reference path="../../_all.ts" />

module usuariosArea {
    export class gruposController {
        static $inject = ['usuariosService', 'usuariosQueryService', 'loginQueryService', 'toasterLite',
            '$mdDialog', '$timeout', '$q', '$log', '$rootScope'];

        constructor(
            private usuariosService: usuariosService,
            private usuariosQueryService: usuariosQueryService,
            private loginQueryService: login.loginQueryService,
            private toasterLite: common.toasterLite,
            private $mdDialog: angular.material.IDialogService,
            private $timeout: ng.ITimeoutService,
            private $q: ng.IQService,
            private $log: ng.ILogService,
            private $rootScope: ng.IRootScopeService
        ) {
            this.recuperarListaDeOrganizaciones();
            this.$rootScope.gruposController = {};
        }
        // loading org
        loaded = false;
        // loading grupos for an org
        gruposLoaded = true;

        filterFromServer = false;
        isDisabled = false;
        searchText: string;
        orgSeleccionada: organizacionDto;

        // list of `organizaciones` value/display objects
        organizaciones: organizacionDto[] = [];

        // list of grupos
        grupos: grupoDto[] = [];

        // ******************************
        // Public methods
        // ******************************

        crearOrganizacion() {
            let idUsuario = this.loginQueryService.tryGetLocalLoginInfo().usuario;
            window.location.replace('#!/usuario/' + idUsuario + '?tab=organizaciones');
        }

        crearNuevoGrupo($event) {
            this.$rootScope.gruposController.orgSeleccionada = this.orgSeleccionada;
            this.$mdDialog.show({
                templateUrl: '../app/dist/usuarios/dialogs/nuevo-grupo-dialog.html',
                parent: angular.element(document.body),
                targetEvent: $event,
                controller: nuevoGrupoDialogController,
                controllerAs: 'vm',
                clickOutsideToClose: true
            }).then((nuevoGrupo: string) => {
                // Agregar nuevo grupo a la lista, si fue exitosa
            }, () => {
                this.toasterLite.info('Creación de grupo cancelada');
            });
        }

        //********************************
        // Internal
        //********************************

        noSePuedeCrearGrupo() {
            return this.orgSeleccionada === null || this.orgSeleccionada === undefined;
        }

        // ******************************
        // Autocomplete stuff
        // ******************************

        private recuperarListaDeOrganizaciones() {
            this.usuariosQueryService.obtenerOrganizaciones(
                response => {
                    this.organizaciones = response.data;
                    this.loaded = true;
                },
                reason => this.toasterLite.error('Hubo un error al recuperar lista de organizaciones', this.toasterLite.delayForever)
            );
        }

        private querySearch(query) {
            var lowercaseQuery = angular.lowercase(query);

            let results = query
                ? this.organizaciones.filter(org => {
                    let coincideConId = (angular.lowercase(org.id).indexOf(lowercaseQuery) > -1);
                    let coincideConDisplay = (angular.lowercase(org.display).indexOf(lowercaseQuery) > -1);
                    return coincideConId || coincideConDisplay;
                })
                : this.organizaciones;

            return results;
        }

        private searchTextChange(text) {
            //this.$log.info('Text changed to ' + text);
        }

        private selectedItemChange(org: organizacionDto) {
            if (org === undefined) {
                // dejo en blanco el filtro
                this.grupos = [];
                return;
            }

            this.gruposLoaded = false;
            this.usuariosQueryService.obtenerGrupos(org.id,
                value => {
                    this.gruposLoaded = true;
                    this.grupos = value.data;
                },
                reason => { this.toasterLite.error('Error al cargar grupos', this.toasterLite.delayForever); });
        }

        /**
         * Create filter function for a query string
         */
        private createFilterFor(query) {
            var lowercaseQuery = angular.lowercase(query);

            return function filterFn(state) {
                return (state.value.indexOf(lowercaseQuery) === 0);
            };

        }
    }
}