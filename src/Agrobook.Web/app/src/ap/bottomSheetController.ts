/// <reference path="../_all.ts" />

module apArea {

    export class bottomSheetController {
        static $inject = ['$mdBottomSheet']

        constructor(
            private $mdBottomSheet: angular.material.IBottomSheetService
        ) {
            this.items = [
                new bottomSheetItem("Nuevo servicio", "nuevoServicio", "add"),
                new bottomSheetItem("Nuevo contrato", "nuevoContrato", "add")
            ];
        }

        items: bottomSheetItem[];
    }

    export class bottomSheetButtonController {
        static $inject = ['$mdBottomSheet']

        constructor(
            private $mdBottomSheet: angular.material.IBottomSheetService
        ) {
        }

        mostrarBottomSheet() {
            this.$mdBottomSheet.show({
                templateUrl: './dist/ap/bottom-sheet.html',
                controller: 'bottomSheetController',
                controllerAs: 'vm'
            }).then(clickedItem => {
            }).catch(error => {
                // User clicked aoutside or hit escape
            });
        }
    }

    class bottomSheetItem {
        constructor(
            public display: string,
            public id: string,
            public icon: string
        ) {
        }
    }
}