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
            this.$rootScope.gruposController = { };
        }

        filterFromServer = false;
        isDisabled = false;
        searchText: string;
        orgSeleccionada: any;

        // list of `organizaciones` value/display objects
        organizaciones = [];

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
                response =>
                {
                    this.organizaciones = response.data.map(org => {
                        return {
                            value: org.id,
                            display: org.display
                        }
                    });
                },
                reason => this.toasterLite.error('Hubo un error al recuperar lista de organizaciones', this.toasterLite.delayForever)
            );
        }

        private querySearch(query) {
            var results = query ? this.organizaciones.filter(this.createFilterFor(query)) : this.organizaciones,
                deferred;
            if (this.filterFromServer) {
                // this just simulates from Server. Add your server filtering here.
                deferred = this.$q.defer();
                this.$timeout(function () { deferred.resolve(results); }, Math.random() * 1000, false);
                return deferred.promise;
            } else {
                return results;
            }
        }

        private searchTextChange(text) {
            //this.$log.info('Text changed to ' + text);
        }

        private selectedItemChange(item) {
            this.$log.info('Item changed to ' + JSON.stringify(item));
            
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