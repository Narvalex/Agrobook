/// <reference path="../_all.ts" />

module archivosArea {
    export class productorDto {
        constructor(
            public id: string,
            public display: string,
            public avatarUrl: string,
            public organizaciones: Organizacion[]
        ) { }
    }

    export class Organizacion {
        constructor(
            public nombre: string,
            public grupos: string
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