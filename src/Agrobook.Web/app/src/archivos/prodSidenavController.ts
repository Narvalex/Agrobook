/// <reference path="../_all.ts" />

module archivosArea {
    export class prodSidenavController {
        static $inject = ['$mdSidenav'];

        constructor(
            private $mdSidenav: angular.material.ISidenavService
        ) {

        }

        toggleSideNav(): void {
            this.$mdSidenav('left').toggle();
        }


    }
}
