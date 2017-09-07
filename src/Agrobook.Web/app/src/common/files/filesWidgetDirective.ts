/// <reference path="../../_all.ts" />

module common {
    // https://weblogs.asp.net/dwahlin/creating-custom-angularjs-directives-part-i-the-fundamentals
    // https://docs.angularjs.org/guide/directive

    export function filesWidgetDirectiveFactory() : ng.IDirective {
        return {
            restrict: 'EA', //E = element, A = attribute, C = class, M = comment
            scope: {
                coleccionId: '=', // a que coleccion pertenece el archivo. {archivos}-{prodId}
                header: '='
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
            var vm = this.$scope;
            vm.scope = this.$scope;
            vm.fileInputId = vm.coleccionId + 'fileInputId';
            vm.addFiles = this.addFiles;
            vm.prepareFiles = this.prepareFiles;
            vm.units = [];
        }

        // two-way binding
        scope: ng.IScope;
        title: string
        coleccionId: string;

        fileInputId: string;
        units: fileUnit[];

        addFiles() {
            document.getElementById(this.fileInputId).click();
        }

        prepareFiles(element: HTMLInputElement) {
            // reset input first
            let container = element.parentElement;
            let content = container.innerHTML;
            container.innerHTML = content;

            // try load to current list;
            this.scope.$apply(scp => {
                let files = element.files;
                for (var i = 0; i < files.length; i++) {
                    let file = files[i];

                    let alreadyExists = false;
                    let newName = file.name; // file.webkitRelativePath could be a name too
                    for (var j = 0; j < scp.units.length; j++) {
                        let existing = scp.units[j];
                        if (existing.name === newName) {
                            console.log('File "' + newName + '" was not added because already exists!');
                            alreadyExists = true;
                            break;
                        }
                    }

                    if (alreadyExists) continue;

                    let unit = new fileUnit(newName, file);
                    scp.units.push(unit);
                    console.log('File "' + newName + '" was added');
                }
            });
        }
    }

    export class fileUnit {
        constructor(
            // icon an such later
            public name: string,
            public file: File
        ) {
        }
    }
}