/// <reference path="../_all.ts" />

module archivosArea {
    export class sidenavController {
        static $inject = ['$mdSidenav', 'toasterLite', '$rootScope', 'config'];

        constructor(
            private $mdSidenav: angular.material.ISidenavService,
            private toasterLite: common.toasterLite,
            private $rootScope: ng.IRootScopeService,
            private config: common.config
        ) {
            this.$rootScope.$on(this.config.eventIndex.archivos.productorSeleccionado, (e, args) => {
                this.idProductor = args;
            });
        }
        
        idProductor: string;

        toggleSideNav(): void {
            this.$mdSidenav('left').toggle();
        }

        nuevoArchivo() {
            this.toasterLite.info(`nuevo archivo para ${this.idProductor}!`);
        }
    }
}
