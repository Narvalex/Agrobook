/// <reference path="../_all.ts" />

module archivosArea {
    export class productorDto {
        constructor(
            public id: string,
            public display: string,
            public avatarUrl: string,
        ) { }
    }

    export class archivoDto {
        constructor(
            public nombre: string,
            public extension: string,
            public fecha: string,
            public desc: string
        ) {
        }
    }
}