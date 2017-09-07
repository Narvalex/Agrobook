/// <reference path="../../_all.ts" />

// this will be a sample
module common {
    // All services are pure functions, without holding state
    export class filesWidgetService {
        static $inject = [];

        constructor(
        ) {
        }

        // we can change the Id if necessary
        selectFiles() {
            setTimeout(() => document.getElementById('awFileInput').click(), 0);
        }

        // we can change the id if necessary
        resetFileInput() {
            let container = document.getElementById('awFileInputContainer');
            let content = container.innerHTML;
            container.innerHTML = content;
        }

        prepareFiles(files: FileList, existing: fileUnit[], onAlreadyExists: (name: string) => any = undefined): fileUnit[] {
            if (onAlreadyExists === undefined)
                onAlreadyExists = (name: string) => console.log(`The file ${name} already exists!`);

            for (var i = 0; i < files.length; i++) {
                let file = files[i];
                let name = file.name; // file.webkitRelativePath could be a name too

                var alreadyExists = false; 


                for (var j = 0; j < existing.length; j++) {
                    if (existing[j].name === name) {
                        onAlreadyExists(file.name);
                        alreadyExists = true;
                        break;
                    }
                }

                if (alreadyExists) continue;

                
                var unit = new fileUnit(name, file);
                existing.push(unit);
            }

            return existing;
        }
    }
}