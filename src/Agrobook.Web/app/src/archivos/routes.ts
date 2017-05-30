/// <reference path="../_all.ts" />

module archivosArea {

    export interface IRouteConfig {
        path: string;
        route: angular.route.IRoute
    }

    export function getRouteConfigs(): IRouteConfig[] {
        return [
            {
                path: '/',
                route: {
                    templateUrl: './dist/archivos/views/main-content.html',
                    reloadOnSearch: false
                }
            },
            {
                path: '/archivos/:idproductor',
                route: {
                    templateUrl: './dist/archivos/views/main-content.html',
                    reloadOnSearch: false
                }
            }
        ];
    }
}