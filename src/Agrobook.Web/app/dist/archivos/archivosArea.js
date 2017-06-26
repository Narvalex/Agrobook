/// <;reference path="../_all.ts" />
var archivosArea;
(function (archivosArea) {
    angular.module('archivosArea', ['ngRoute', 'ngMaterial', 'ngMdIcons'])
        .value('config', new common.config())
        .service('localStorageLite', common.localStorageLite)
        .service('toasterLite', common.toasterLite)
        .service('httpLite', common.httpLite)
        .service('loginService', login.loginService)
        .service('uploadService', archivosArea.uploadService)
        .service('loginQueryService', login.loginQueryService)
        .service('archivosQueryService', archivosArea.archivosQueryService)
        .service('usuariosQueryService', usuariosArea.usuariosQueryService)
        .controller('userMenuWidgetController', common.userMenuWidgetController)
        .controller('sidenavController', archivosArea.sidenavController)
        .controller('prodSidenavController', archivosArea.prodSidenavController)
        .controller('toolbarHeaderController', archivosArea.toolbarHeaderController)
        .controller('uploadCenterController', archivosArea.uploadCenterController)
        .controller('mainContentController', archivosArea.mainContentController)
        .config(['$mdIconProvider', '$mdThemingProvider', '$httpProvider', '$routeProvider',
        '$mdDateLocaleProvider', function ($mdIconProvider, $mdThemingProvider, $httpProvider, $routeProvider, $mdDateLocaleProvider) {
            // most from flat icon dot com
            $mdIconProvider
                .defaultIconSet('./assets/svg/avatars.svg', 128)
                .icon('menu', './assets/svg/menu.svg', 24)
                .icon('close', './assets/svg/close.svg');
            $mdThemingProvider.theme('default')
                .primaryPalette('green')
                .accentPalette('blue');
            common.registerHttpInterceptors($httpProvider);
            archivosArea.getRouteConfigs().forEach(function (config) {
                $routeProvider.when(config.path, config.route);
            });
            $routeProvider.otherwise({ redirectTo: '/' });
            configLang($mdDateLocaleProvider);
        }]);
    function configLang($mdDateLocaleProvider) {
        $mdDateLocaleProvider.months = [
            'enero', 'febrero', 'marzo',
            'abril', 'mayo', 'junio',
            'julio', 'agosto', 'septiembre',
            'octubre', 'noviembre', 'diciembre'
        ];
        $mdDateLocaleProvider.shortMonths = [
            'ene', 'feb', 'mar',
            'abr', 'may', 'jun',
            'jul', 'ago', 'sep',
            'oct', 'nov', 'dic'
        ];
        $mdDateLocaleProvider.days = [
            'lunes', 'martes', 'miercoles', 'jueves', 'viernes',
            'sabado', 'domingo'
        ];
        $mdDateLocaleProvider.shortDays = [
            'Lu', 'Ma', 'Mi', 'Je', 'Vi', 'Sa', 'Do'
        ];
        $mdDateLocaleProvider.parseDate = function (dateString) {
            var m = moment(dateString, 'L', true);
            return m.isValid() ? m.toDate() : new Date(NaN);
        };
        $mdDateLocaleProvider.formatDate = function (date) {
            //return moment(date).format('L');
            return moment(date).format("DD/MM/YYYY");
        };
        $mdDateLocaleProvider.monthHeaderFormatter = function (date) {
            return $mdDateLocaleProvider.shortMonths[date.getMonth()] + ' ' + date.getFullYear();
        };
        $mdDateLocaleProvider.weekNumberFormatter = function (weekNumber) {
            return 'Semana ' + weekNumber;
        };
        $mdDateLocaleProvider.msgCalendar = 'Calendario';
        $mdDateLocaleProvider.msgOpenCalendar = 'Abrir calendario';
    }
})(archivosArea || (archivosArea = {}));
//# sourceMappingURL=archivosArea.js.map