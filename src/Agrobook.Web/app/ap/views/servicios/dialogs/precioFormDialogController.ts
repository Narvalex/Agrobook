/// <reference path="../../../../_all.ts" />

module apArea {
    export class precioFormDialogController {

        static $inject = ['$mdDialog'];

        constructor(
            private $mdDialog: angular.material.IDialogService
        ) {
        }
    }
}