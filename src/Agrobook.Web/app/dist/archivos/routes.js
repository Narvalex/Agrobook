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
                path: '/archivos/:idproductor',
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