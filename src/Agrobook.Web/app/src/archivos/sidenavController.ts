/// <reference path="../_all.ts" />

module archivosArea {
    export interface IWindowFileReady extends Window {
        File: File,
        FileList: FileList,
        FileReader: FileReader
    }

    export class sidenavController {
        static $inject = ['$mdSidenav', 'toasterLite', '$rootScope', 'config', '$mdDialog'];

        constructor(
            private $mdSidenav: angular.material.ISidenavService,
            private toasterLite: common.toasterLite,
            private $rootScope: ng.IRootScopeService,
            private config: common.config,
            private $mdDialog: angular.material.IDialogService
        ) {
            this.$rootScope.$on(this.config.eventIndex.archivos.productorSeleccionado, (e, args) => {
                this.idProductor = args;
            });
        }

        idProductor: string;

        toggleSideNav(): void {
            this.$mdSidenav('left').toggle();
        }

        mostrarUploadCenter($event) {
            //this.toasterLite.info(`nuevo archivo para ${this.idProductor}!`);
            this.$mdDialog.show({
                templateUrl: '../app/dist/archivos/dialogs/upload-center-dialog.html',
                parent: angular.element(document.body),
                targetEvent: $event,
                clickOutsideToClose: false,
                fullscreen: true
            })
            .then(answer => {
                console.log('Modal aceptado...');
            }, () => {
            });
        }
    }
}
