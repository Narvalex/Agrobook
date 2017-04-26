/// <reference path="../_all.ts" />

module usuariosArea {
    export class usuarioDto {
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

    export class actualizarPerfilDto {
        constructor(
            public avatarUrl: string,
            public nombreParaMostrar: string,
            public passwordActual: string,
            public nuevoPassword: string
        ){
        }
    }
}