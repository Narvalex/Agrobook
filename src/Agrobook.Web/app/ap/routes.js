/// <reference path="../_all.ts" />
var apArea;
(function (apArea) {
    function getRouteConfigs() {
        return [
            {
                path: '/',
                route: {
                    templateUrl: './views/main-content.html',
                    reloadOnSearch: false
                }
            },
            {
                path: '/prod/:idProd',
                route: {
                    templateUrl: './views/prod/prod-main-content.html',
                    reloadOnSearch: false
                }
            },
            {
                path: '/org/:idOrg',
                route: {
                    templateUrl: './views/org/org-main-content.html',
                    reloadOnSearch: false
                }
            },
            {
                path: '/servicios/:idProd/:idServicio',
                route: {
                    templateUrl: './views/servicios/servicio-main-content.html',
                    reloadOnSearch: false
                }
            },
            {
                path: '/reportes',
                route: {
                    templateUrl: './views/reportes/reportes-main-content.html',
                    reloadOnSearch: false
                }
            }
        ];
    }
    apArea.getRouteConfigs = getRouteConfigs;
})(apArea || (apArea = {}));
//# sourceMappingURL=routes.js.map