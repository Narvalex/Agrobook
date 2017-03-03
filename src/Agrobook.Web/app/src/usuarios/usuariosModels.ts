/// <reference path="../_all.ts" />

module UsuariosArea {
    export class Usuario {
        constructor(
            public avatarUrl: string,
            public nombreDeUsuario: string,
            public password: string
        ) { }
    }
}