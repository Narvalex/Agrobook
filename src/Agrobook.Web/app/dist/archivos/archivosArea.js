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
            configureIcons($mdIconProvider);
            $mdThemingProvider.theme('default')
                .primaryPalette('green')
                .accentPalette('blue');
            // optional themes
            $mdThemingProvider.theme('dark-grey').backgroundPalette('grey').dark();
            $mdThemingProvider.theme('dark-orange').backgroundPalette('orange').dark();
            $mdThemingProvider.theme('dark-purple').backgroundPalette('deep-purple').dark();
            $mdThemingProvider.theme('dark-blue').backgroundPalette('blue').dark();
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
    function configureIcons($mdIconProvider) {
        // Credits 
        // most from flaticon.com
        // https://icons8.com/icon/11591/microsoft-word#filled
        $mdIconProvider
            .defaultIconSet('./assets/svg/avatars.svg', 128)
            .icon('menu', './assets/svg/menu.svg', 24)
            .icon('close', './assets/svg/close.svg')
            .icon('pdf', './assets/svg/pdf.svg')
            .icon('list', './assets/svg/list.svg')
            .icon('instagram', './assets/svg/instagram.svg')
            .icon('picture', './assets/svg/picture.svg')
            .icon('google-earth', './assets/svg/google-earth.svg')
            .icon('generic-file', './assets/svg/generic-file.svg')
            .icon('excel', './assets/svg/excel.svg')
            .icon('word', './assets/svg/word.svg')
            .icon('powerPoint', './assets/svg/power-point.svg');
    }
})(archivosArea || (archivosArea = {}));
//# sourceMappingURL=archivosArea.js.map