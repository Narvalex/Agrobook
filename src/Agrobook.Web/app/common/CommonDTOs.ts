/// <reference path="../_all.ts" />

module common {
    export class perfilActualizado {
        constructor(
            public usuario: string,
            public avatarUrl: string,
            public nombreParaMostrar: string,
            public telefono: string, 
            public email: string
        ) {
        }
    }
}