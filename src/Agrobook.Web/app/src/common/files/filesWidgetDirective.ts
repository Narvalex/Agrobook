/// <reference path="../../_all.ts" />

module common {
    // https://weblogs.asp.net/dwahlin/creating-custom-angularjs-directives-part-i-the-fundamentals
    // https://docs.angularjs.org/guide/directive

    export function filesWidgetDirectiveFactory() : ng.IDirective {
        return {
            restrict: 'EA', //E = element, A = attribute, C = class, M = comment
            scope: {
                coleccionId: '=' // a que coleccion pertenece el archivo. {archivos}-{prodId}
            },
            templateUrl: './dist/common/files/files-widget.html',
            controller: filesWidgetController //Embed a custom controller in the directive,
        };
    }

    class filesWidgetController {
        static $inject = ['$scope'];

        constructor(
            private $scope: ng.IScope
        ) {
            console.log('cargando coleccion id: ' + this.$scope.coleccionId);
            this.$scope.fileInputId = this.$scope.coleccionId + 'fileInputId';
            this.$scope.fileInputIdContainer = this.$scope.coleccionId + 'fileInputIdContainer';
        }
    }
}