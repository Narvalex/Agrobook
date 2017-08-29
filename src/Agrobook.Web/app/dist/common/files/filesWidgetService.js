/// <reference path="../../_all.ts" />
// this will be a sample
var common;
(function (common) {
    // All services are pure functions, without holding state
    var filesWidgetService = (function () {
        function filesWidgetService() {
        }
        // we can change the Id if necessary
        filesWidgetService.prototype.selectFiles = function () {
            setTimeout(function () { return document.getElementById('awFileInput').click(); }, 0);
        };
        // we can change the id if necessary
        filesWidgetService.prototype.resetFileInput = function () {
            var container = document.getElementById('awFileInputContainer');
            var content = container.innerHTML;
            container.innerHTML = content;
        };
        filesWidgetService.prototype.prepareFiles = function (files, existing, onAlreadyExists) {
            if (onAlreadyExists === void 0) { onAlreadyExists = undefined; }
            if (onAlreadyExists === undefined)
                onAlreadyExists = function (name) { return console.log("The file " + name + " already exists!"); };
            for (var i = 0; i < files.length; i++) {
                var file = files[i];
                var name_1 = file.name; // file.webkitRelativePath could be a name too
                var alreadyExists = false;
                for (var j = 0; j < existing.length; j++) {
                    if (existing[j].name === name_1) {
                        onAlreadyExists(file.name);
                        alreadyExists = true;
                        break;
                    }
                }
                if (alreadyExists)
                    continue;
                var unit = new fileUnit(name_1, file);
                existing.push(unit);
            }
            return existing;
        };
        return filesWidgetService;
    }());
    filesWidgetService.$inject = [];
    common.filesWidgetService = filesWidgetService;
    var fileUnit = (function () {
        function fileUnit(name, file) {
            this.name = name;
            this.file = file;
        }
        return fileUnit;
    }());
    common.fileUnit = fileUnit;
})(common || (common = {}));
//# sourceMappingURL=filesWidgetService.js.map