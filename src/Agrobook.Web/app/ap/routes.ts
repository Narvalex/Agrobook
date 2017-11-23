/// <reference path="../_all.ts" />

module apArea {

    export interface IRouteConfig {
        path: string;
        route: angular.route.IRoute
    }

    export function getRouteConfigs(): IRouteConfig[] {
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
}