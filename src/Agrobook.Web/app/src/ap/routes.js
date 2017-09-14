/// <reference path="../_all.ts" />
var apArea;
(function (apArea) {
    function getRouteConfigs() {
        return [
            {
                path: '/',
                route: {
                    templateUrl: './src/ap/views/main-content.html',
                    reloadOnSearch: false
                }
            },
            {
                path: '/prod/:idProd',
                route: {
                    templateUrl: './src/ap/views/prod/prod-main-content.html',
                    reloadOnSearch: false
                }
            },
            {
                path: '/org/:idOrg',
                route: {
                    templateUrl: './src/ap/views/org/org-main-content.html',
                    reloadOnSearch: false
                }
            },
            {
                path: '/servicios/:idProd/:idServicio',
                route: {
                    templateUrl: './src/ap/views/servicios/servicio-main-content.html',
                    reloadOnSearch: false
                }
            }
        ];
    }
    apArea.getRouteConfigs = getRouteConfigs;
})(apArea || (apArea = {}));
//# sourceMappingURL=routes.js.map