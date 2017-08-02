/// <reference path="../_all.ts" />
var apArea;
(function (apArea) {
    function getRouteConfigs() {
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
    apArea.getRouteConfigs = getRouteConfigs;
})(apArea || (apArea = {}));
//# sourceMappingURL=routes.js.map