/// <reference path="../_all.ts" />
var archivosArea;
(function (archivosArea) {
    function getRouteConfigs() {
        return [
            {
                path: '/',
                route: {
                    templateUrl: './dist/archivos/views/main-content.html',
                    reloadOnSearch: false
                }
            },
            {
                path: '/archivos/:idProductor',
                route: {
                    templateUrl: './dist/archivos/views/main-content.html',
                    reloadOnSearch: false
                }
            },
            {
                path: '/upload/:idProductor',
                route: {
                    templateUrl: './dist/archivos/views/main-content.html',
                    reloadOnSearch: false
                }
            }
        ];
    }
    archivosArea.getRouteConfigs = getRouteConfigs;
})(archivosArea || (archivosArea = {}));
//# sourceMappingURL=routes.js.map