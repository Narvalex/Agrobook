/// <reference path="../_all.ts" />

module usuariosArea {

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
                path: '/usuario/:idUsuario',
                route: {
                    templateUrl: './views/main-content.html',
                    reloadOnSearch: false
                }
            }
        ];
    }
}