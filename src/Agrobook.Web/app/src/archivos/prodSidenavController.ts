/// <reference path="../_all.ts" />

module archivosArea {
    export class prodSidenavController {
        static $inject = ['$mdSidenav', 'toasterLite', 'archivosQueryService'];

        constructor(
            private $mdSidenav: angular.material.ISidenavService,
            private toasterLite: common.toasterLite,
            private archivosQueryService: archivosQueryService
        ) {
            this.cargarListaDeProductores();
        }

        loaded: boolean = false;
        productores: productorDto[];

        toggleSideNav(): void {
            this.$mdSidenav('right').toggle();
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
