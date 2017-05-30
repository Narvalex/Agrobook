/// <reference path="../../_all.ts" />

module archivosArea {
    export class mainContentController {
        static $inject = ['$mdSidenav'];

        constructor(
            private $mdSidenav: angular.material.ISidenavService
        ) {

        }

        toggleSideNav(): void {
            this.$mdSidenav('right').toggle();
        }

        isSideNavOpen(): boolean {
            var open = this.$mdSidenav('right').isOpen();
            return open;
        }
    }
}