/// <reference path="../_all.ts" />

module usuariosArea {

    export interface IRouteConfig {
        path: string;
        route: angular.route.IRoute
    }

    export function getRouteConfigs() : IRouteConfig[] {
        return [
            { 
                path: '/',
                route: {
                    templateUrl: './dist/usuarios/views/no-user.html',
                    reloadOnSearch: false
                }
            },
            {
                path: '/usuario/:idUsuario',
                route: {
                    templateUrl: './dist/usuarios/views/usuario.html',
                    reloadOnSearch: false
                }
            }
        ];
    }
}