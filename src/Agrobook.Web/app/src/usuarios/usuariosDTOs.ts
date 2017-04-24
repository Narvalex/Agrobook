/// <reference path="../_all.ts" />

module usuariosArea {
    export class UsuarioDto {
        constructor(
            public avatarUrl: string,
            public nombreDeUsuario: string,
            public nombreParaMostrar: string,
            public password: string,
            public claims: string[]
        ) { }
    }

    export class usuarioInfoBasica {
        constructor(
            public nombre: string,
            public nombreParaMostrar: string,
            public avatarUrl: string
        ) {
        }
    }
}