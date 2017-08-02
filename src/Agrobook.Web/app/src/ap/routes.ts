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
                    templateUrl: './dist/ap/views/main-content.html',
                    reloadOnSearch: false
                }
            }
        ];
    }
}