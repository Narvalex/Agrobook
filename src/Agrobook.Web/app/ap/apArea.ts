/// <reference path="../_all.ts" />

module apArea {
    angular.module('apArea', ['ngRoute', 'ngMaterial', 'ngMdIcons'])
        .value('config', new common.config())
        // Services
        .service('toasterLite', common.toasterLite)
        .service('localStorageLite', common.localStorageLite)
        .service('loginService', login.loginService) // to log out...! do not remove this!
        .service('loginQueryService', login.loginQueryService)
        .service('apQueryService', apQueryService)
        .service('apService', apService)
        .service('fakeDb', fakeDb)
        // Directives
        .directive('fileWidget', common.filesWidgetDirectiveFactory)
        // Controllers
        .controller('sidenavController', sidenavController)
        .controller('bottomSheetButtonController', bottomSheetButtonController)
        .controller('bottomSheetController', bottomSheetController)
        .controller('toolbarHeaderController', toolbarHeaderController)
        .controller('userMenuWidgetController', common.userMenuWidgetController)
        .controller('mainContentController', mainContentController)
        // org controllers
        .controller('orgMainContentController', orgMainContentController)
        .controller('orgTabServiciosController', orgTabServiciosController)
        .controller('orgTabContratosController', orgTabContratosController)
        // prod controllers
        .controller('prodMainContentController', prodMainContentController)
        .controller('prodTabParcelasController', prodTabParcelasController)
        .controller('prodTabServiciosController', prodTabServiciosController)
        // servicios controllers
        .controller('serviciosMainContentController', serviciosMainContentController)
        .controller('serviciosTabResumenController', serviciosTabResumenController)
        .controller('serviciosTabParcelaController', serviciosTabParcelaController)
        .controller('serviciosTabDiagnosticoController', serviciosTabDiagnosticoController)
        .controller('serviciosTabPrescripcionesController', serviciosTabPrescripcionesController)
        // config
        .config(['$mdIconProvider', '$mdThemingProvider', '$httpProvider', '$routeProvider',
            '$mdDateLocaleProvider', (
            $mdIconProvider: angular.material.IIconProvider,
            $mdThemingProvider: angular.material.IThemingProvider,
            $httpProvider: angular.IHttpProvider,
            $routeProvider: angular.route.IRouteProvider,
            $mdDateLocaleProvider: angular.material.IDateLocaleProvider
        ) => {
            // most from flat icon dot com
                configureIcons($mdIconProvider);

            $mdThemingProvider.theme('default')
                .primaryPalette('green')
                .accentPalette('blue');

            common.registerHttpInterceptors($httpProvider);

            getRouteConfigs().forEach(config => {
                $routeProvider.when(config.path, config.route);
            });
            $routeProvider.otherwise({ redirectTo: '/' });

            configLang($mdDateLocaleProvider);
        }])
        .run(['$rootScope', (
            $rootScope: angular.IRootScopeService
        ) => {
            // Aqui se pueden definir DTOS compartidos. Algo asi como "variables globales"
        }]);

    function configLang($mdDateLocaleProvider: angular.material.IDateLocaleProvider) {


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

    function configureIcons($mdIconProvider: angular.material.IIconProvider) {
        // Credits 
        // most from flaticon.com
        // https://icons8.com/icon/11591/microsoft-word#filled
        $mdIconProvider
            .defaultIconSet('../assets/svg/avatars.svg', 128)
            .icon('menu', '../assets/svg/menu.svg', 24)
            .icon('close', '../assets/svg/close.svg')
            .icon('agrobook-white', '../assets/svg/agrobook-white.svg')
            .icon('agrobook-green', '../assets/svg/agrobook-green.svg')
            // Archivos
            .icon('pdf', '../assets/svg/pdf.svg')
            .icon('list', '../assets/svg/list.svg')
            .icon('instagram', '../assets/svg/instagram.svg')
            .icon('picture', '../assets/svg/picture.svg')
            .icon('google-earth', '../assets/svg/google-earth.svg')
            .icon('generic-file', '../assets/svg/generic-file.svg')
            .icon('excel', '../assets/svg/excel.svg')
            .icon('word', '../assets/svg/word.svg')
            .icon('powerPoint', '../assets/svg/power-point.svg');
    }
}