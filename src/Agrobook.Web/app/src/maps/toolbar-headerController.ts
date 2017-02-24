/// <reference path="../_all.ts" />

module MapsArea {
    export class ToolbarHeaderController {
        static $inject = ['$mdSidenav'];

        constructor(
            private $mdSidenav: angular.material.ISidenavService)
        {
        }

        toggleSideNav(): void {
            this.$mdSidenav('left').toggle();
        }
    }
}