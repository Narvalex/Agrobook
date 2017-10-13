/// <reference path="../_all.ts" />

module apArea {

    export class bottomSheetController {
        static $inject = ['$mdBottomSheet', '$mdSidenav']

        constructor(
            private $mdBottomSheet: angular.material.IBottomSheetService,
            private $mdSidenav: angular.material.ISidenavService
        ) {
            this.items = [
                new bottomSheetItem("Ir al inicio de Agricultura de Precisión", "home", './index.html#!/')
            ];
        }

        items: bottomSheetItem[];

        goTo(item: bottomSheetItem) {
            window.location.replace(item.url);
            this.$mdSidenav('left').close();
            this.$mdBottomSheet.hide();
        }
    }

    export class bottomSheetButtonController {
        static $inject = ['$mdBottomSheet']

        constructor(
            private $mdBottomSheet: angular.material.IBottomSheetService
        ) {
        }

        mostrarBottomSheet() {
            this.$mdBottomSheet.show({
                templateUrl: './bottom-sheet.html',
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
            public icon: string,
            public url: string
        ) {
        }
    }
}