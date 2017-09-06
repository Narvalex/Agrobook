/// <reference path="../../_all.ts" />
var common;
(function (common) {
    // https://weblogs.asp.net/dwahlin/creating-custom-angularjs-directives-part-i-the-fundamentals
    // https://docs.angularjs.org/guide/directive
    function filesWidgetDirectiveFactory() {
        return {
            restrict: 'EA',
            scope: {
                coleccionId: '=' // a que coleccion pertenece el archivo. {archivos}-{prodId}
            },
            templateUrl: './dist/common/files/files-widget.html',
            controller: filesWidgetController //Embed a custom controller in the directive,
        };
    }
    common.filesWidgetDirectiveFactory = filesWidgetDirectiveFactory;
    var filesWidgetController = (function () {
        function filesWidgetController($scope) {
            this.$scope = $scope;
            console.log('cargando coleccion id: ' + this.$scope.coleccionId);
            this.$scope.fileInputId = this.$scope.coleccionId + 'fileInputId';
            this.$scope.fileInputIdContainer = this.$scope.coleccionId + 'fileInputIdContainer';
        }
        return filesWidgetController;
    }());
    filesWidgetController.$inject = ['$scope'];
})(common || (common = {}));
//# sourceMappingURL=filesWidgetDirective.js.map