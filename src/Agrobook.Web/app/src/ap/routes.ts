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
}