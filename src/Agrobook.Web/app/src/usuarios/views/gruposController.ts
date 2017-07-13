/// <reference path="../../_all.ts" />

module usuariosArea {
    export class gruposController {
        static $inject = ['usuariosService', 'usuariosQueryService', 'loginQueryService', 'toasterLite',
            '$mdDialog', '$timeout', '$q', '$log', '$rootScope', '$scope', '$routeParams', 'config'];

        constructor(
            private usuariosService: usuariosService,
            private usuariosQueryService: usuariosQueryService,
            private loginQueryService: login.loginQueryService,
            private toasterLite: common.toasterLite,
            private $mdDialog: angular.material.IDialogService,
            private $timeout: ng.ITimeoutService,
            private $q: ng.IQService,
            private $log: ng.ILogService,
            private $rootScope: ng.IRootScopeService,
            private $scope: ng.IScope,
            private $routeParams: ng.route.IRouteParamsService,
            private config: common.config
        ) {
            this.idUsuario = this.$routeParams['idUsuario'];
            if (this.idUsuario === undefined)
                this.idUsuario = this.loginQueryService.tryGetLocalLoginInfo().usuario;

            this.recuperarListaDeOrganizaciones();
            this.$rootScope.gruposController = {};

            this.$scope.$on(this.config.eventIndex.usuarios.usuarioAgregadoAOrganizacion, (e, args: { idUsuario: string, org: organizacionDto }) => {
                if (this.idUsuario === args.idUsuario) {
                    // angular le da una propiedad nueva al objeto que parece que le corrompe. Por eso creo uno  nuevo.
                    var dto = new organizacionDto(args.org.id, args.org.display, false);
                    this.organizaciones.push(dto);
                }
            });
        }
        // loading org
        loaded = false;
        // loading grupos for an org
        gruposLoaded = true;
        agregandoAGrupo = false;

        idUsuario: string;

        creandoGrupo = false;

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
            this.creandoGrupo = true;
            this.$rootScope.gruposController.orgSeleccionada = this.orgSeleccionada;
            this.$mdDialog.show({
                templateUrl: '../app/dist/usuarios/dialogs/nuevo-grupo-dialog.html',
                parent: angular.element(document.body),
                targetEvent: $event,
                controller: nuevoGrupoDialogController,
                controllerAs: 'vm',
                clickOutsideToClose: true
            }).then((nuevoGrupo: grupoDto) => {
                // Agregar nuevo grupo a la lista, si fue exitosa
                this.grupos.push(nuevoGrupo);
                this.creandoGrupo = false;
            }, () => {
                this.toasterLite.info('Creación de grupo cancelada');
                this.creandoGrupo = false;
            });
        }

        agregarAGrupo($event, grupo: grupoDto) {
            this.agregandoAGrupo = true;
            this.usuariosService.agregarUsuarioAGrupo(this.idUsuario, this.orgSeleccionada.id, grupo.id,
                value => {
                    for (var i = 0; i < this.grupos.length; i++) {
                        if (this.grupos[i].id == grupo.id) {
                            this.grupos[i].usuarioEsMiembro = true;
                            break;
                        }
                    }

                    this.toasterLite.success(`El usuario ${this.idUsuario} a sido agregado al grupo ${grupo.display}`);
                    this.agregandoAGrupo = false;
                },
                reason => {
                    this.toasterLite.error('Hubo un error al intentar agregar usuario al grupo seleccionado', this.toasterLite.delayForever);
                    this.agregandoAGrupo = false;
                });
        }

        irAOrgTab() {
            // TODO...
        }

        //********************************
        // Internal
        //********************************

        // ******************************
        // Autocomplete stuff
        // ******************************

        private recuperarListaDeOrganizaciones() {
            this.usuariosQueryService.obtenerOrganizacionesDelUsuario(
                this.idUsuario,
                response => {
                    let lista: organizacionDto[] = [];
                    for (var i = 0; i < response.data.length; i++) {
                        lista.push(new organizacionDto(response.data[i].id, response.data[i].display, response.data[i].usuarioEsMiembro)); 
                    }
                    this.organizaciones = lista;
                    this.loaded = true;
                },
                reason => this.toasterLite.error('Hubo un error al recuperar lista de organizaciones', this.toasterLite.delayForever)
            );
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
            this.usuariosQueryService.obtenerGrupos(org.id, this.idUsuario,
                value => {
                    this.gruposLoaded = true;
                    this.grupos = value.data;
                },
                reason => { this.toasterLite.error('Error al cargar grupos', this.toasterLite.delayForever); });
        }

        private refreshOrgList(): organizacionDto[] {
            return this.organizaciones;
        }
    }
}