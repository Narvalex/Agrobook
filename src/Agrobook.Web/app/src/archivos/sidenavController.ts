/// <reference path="../_all.ts" />

module archivosArea {
    export class sidenavController {
        static $inject = ['$mdSidenav', 'usuariosQueryService', 'toasterLite', '$rootScope', 'config', '$mdDialog',
            'loginService'
        ];

        constructor(
            private $mdSidenav: angular.material.ISidenavService,
            private usuariosQueryService: usuariosArea.usuariosQueryService,
            private toasterLite: common.toasterLite,
            private $rootScope: ng.IRootScopeService,
            private config: common.config,
            private $mdDialog: angular.material.IDialogService,
            private loginService: login.loginService
        ) {
            // Auth
            var roles = this.config.claims.roles;
            this.puedeCargarArchivos = this.loginService.autorizar([roles.Gerente, roles.Tecnico]);


            this.$rootScope.$on(this.config.eventIndex.archivos.productorSeleccionado, (e, args) => {
                this.idProductor = args;
                this.recuperarInfoDelProductor();
            });
            this.$rootScope.$on(this.config.eventIndex.archivos.abrirCuadroDeCargaDeArchivos, (e, args) => {
                this.idProductor = args;
                this.recuperarInfoDelProductor();
                this.initializeUploadCenter();
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

        // Auth
        puedeCargarArchivos: boolean = false;

        idProductor: string;
        productor: productorDto;
        eventoCarga = null;

        // Filtraje
        filtros: FiltroDto[];

        toggleSideNav(): void {
            this.$mdSidenav('left').toggle();
        }

        mostrarDialogoDeCarga($event) {
            this.eventoCarga = $event;
            if (location.hash.slice(3, 9) === 'upload')
                this.initializeUploadCenter();
            else
                window.location.replace('#!/upload/' + this.idProductor);
        }

        // INTERNAL

        private initializeUploadCenter() {
            //this.toasterLite.info(`nuevo archivo para ${this.idProductor}!`);
            this.$mdDialog.show({
                templateUrl: '../app/dist/archivos/dialogs/upload-center-dialog.html',
                parent: angular.element(document.body),
                targetEvent: this.eventoCarga,
                clickOutsideToClose: true,
                fullscreen: true
            })
                .then(answer => {
                    console.log('Modal aceptado...');
                }, () => {
                });
        }

        private recuperarInfoDelProductor() {
            this.usuariosQueryService.obtenerInfoBasicaDeUsuario(this.idProductor,
                value => {
                    this.productor = new productorDto(value.data.nombre, value.data.nombreParaMostrar, value.data.avatarUrl, null);
                },
                reason => {
                    this.toasterLite.error('Ocurrió un error al recuperar información del usuario', this.toasterLite.delayForever);
                });
        }
    }

    export interface IWindowFileReady extends Window {
        File: File,
        FileList: FileList,
        FileReader: FileReader
    }

    export class FiltroDto {
        constructor(
            public display: string,
            public icon: string
        ) { }
    }
}
