/// <reference path="../../_all.ts" />
var common;
(function (common) {
    // https://weblogs.asp.net/dwahlin/creating-custom-angularjs-directives-part-i-the-fundamentals
    // https://docs.angularjs.org/guide/directive
    function filesWidgetDirectiveFactory() {
        return {
            restrict: 'EA',
            scope: {
                coleccionId: '=',
                header: '='
            },
            templateUrl: './dist/common/files/files-widget.html',
            controller: filesWidgetController //Embed a custom controller in the directive,
        };
    }
    common.filesWidgetDirectiveFactory = filesWidgetDirectiveFactory;
    var filesWidgetController = (function () {
        function filesWidgetController($scope) {
            this.$scope = $scope;
            var vm = this.$scope;
            vm.scope = this.$scope;
            vm.fileInputId = vm.coleccionId + 'fileInputId';
            vm.addFiles = this.addFiles;
            vm.prepareFiles = this.prepareFiles;
            vm.units = [];
        }
        filesWidgetController.prototype.addFiles = function () {
            document.getElementById(this.fileInputId).click();
        };
        filesWidgetController.prototype.prepareFiles = function (element) {
            // reset input first
            var container = element.parentElement;
            var content = container.innerHTML;
            container.innerHTML = content;
            // try load to current list;
            this.scope.$apply(function (scp) {
                var files = element.files;
                for (var i = 0; i < files.length; i++) {
                    var file = files[i];
                    var alreadyExists = false;
                    var newName = file.name; // file.webkitRelativePath could be a name too
                    for (var j = 0; j < scp.units.length; j++) {
                        var existing = scp.units[j];
                        if (existing.name === newName) {
                            console.log('File "' + newName + '" was not added because already exists!');
                            alreadyExists = true;
                            break;
                        }
                    }
                    if (alreadyExists)
                        continue;
                    var unit = new fileUnit(newName, file);
                    scp.units.push(unit);
                    console.log('File "' + newName + '" was added');
                }
            });
        };
        return filesWidgetController;
    }());
    filesWidgetController.$inject = ['$scope'];
    var fileUnit = (function () {
        function fileUnit(
            // icon an such later
            name, file) {
            this.name = name;
            this.file = file;
        }
        return fileUnit;
    }());
    common.fileUnit = fileUnit;
})(common || (common = {}));
//# sourceMappingURL=filesWidgetDirective.js.map