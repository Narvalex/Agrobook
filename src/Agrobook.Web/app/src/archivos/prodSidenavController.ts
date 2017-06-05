/// <reference path="../_all.ts" />

module archivosArea {
    export class prodSidenavController {
        static $inject = ['$mdSidenav', 'toasterLite', 'archivosQueryService', '$routeParams'];

        constructor(
            private $mdSidenav: angular.material.ISidenavService,
            private toasterLite: common.toasterLite,
            private archivosQueryService: archivosQueryService,
            private $routeParams: ng.route.IRouteParamsService
        ) {
            var idProductor = this.$routeParams['idProductor'];
            if (idProductor === undefined) {
                // No hay productor seleccionado, pedir uno
                this.toggleSideNav();
                this.title = 'Seleccione un productor...'
            }
            else {
                this.title = 'Productores'
            }   

            this.cargarListaDeProductores();
        }

        title = '';

        loaded: boolean = false;
        productores: productorDto[]; 
        productorSeleccionado: productorDto;

        toggleSideNav(): void {
            this.$mdSidenav('right').toggle();
        }

        seleccionarProductor(productor: productorDto) {
            this.toggleSideNav(); 
            this.productorSeleccionado = productor;
            window.location.replace('#!/archivos/' + productor.id);
        }

        // Internal

        private cargarListaDeProductores() {
            this.archivosQueryService.obtenerListaDeProductores(
                value => {
                    this.productores = value.data;
                    this.loaded = true;
                },
                reason => {
                    this.toasterLite.error('Hubo un error al recuperar la lista de productores.', this.toasterLite.delayForever);
                });
        }
    }
}
