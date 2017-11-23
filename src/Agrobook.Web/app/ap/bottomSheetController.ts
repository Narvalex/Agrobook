/// <reference path="../_all.ts" />

module apArea {

    export class bottomSheetController {
        static $inject = ['$mdBottomSheet', '$mdSidenav', 'loginService', 'config'];

        constructor(
            private $mdBottomSheet: angular.material.IBottomSheetService,
            private $mdSidenav: angular.material.ISidenavService,
            private loginService: login.loginService,
            private config: common.config
        ) {
            this.items = [
                new bottomSheetItem("Ir al inicio de Agricultura de Precisión", "home", './index.html#!/')
            ];

            let claims = this.config.claims; 
            this.puedeVerReportes = this.loginService.autorizar([claims.roles.Gerente, claims.roles.Tecnico]);

            if (this.puedeVerReportes)
                this.items.push(new bottomSheetItem('Ver Reportes', 'description', './index.html#!/reportes'));
        }

        puedeVerReportes: boolean;
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