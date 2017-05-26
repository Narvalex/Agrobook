/// <reference path="../../_all.ts" />

module usuariosArea {
    export class nuevoUsuarioDialogController {

        static $inject = ['$mdDialog', 'usuariosService', 'usuariosQueryService', 'toasterLite', 'config'];

        constructor(
            private $mdDialog: angular.material.IDialogService,
            private usuariosService: usuariosArea.usuariosService,
            private usuariosQueryService: usuariosArea.usuariosQueryService,
            private toasterLite: common.toasterLite,
            private config: common.config
        ) {
            this.setDefaultSubmitText();
            this.avatarUrls = config.avatarUrls; 
            this.obtenerListaDeClaims();
        }

        claimsLoaded = false;
        avatarUrls = [];  

        // todos los claims cargados
        claims: claimDto[]; 

        usuario: usuarioDto;
        claimsSeleccionados: claimDto[] = [];

        bloquearSubmit: boolean = false;
        submitLabel: string;

        cancelar(): void {
            this.$mdDialog.cancel();
        }

        agregarClaim(claim: claimDto) {
            for (var i = 0; i < this.claimsSeleccionados.length; i++) {
                if (this.claimsSeleccionados[i].id === claim.id)
                    return;
            }
            this.claimsSeleccionados.push(claim);
        }

        quitarClaim(claim: claimDto) {
            for (var i = 0; i < this.claimsSeleccionados.length; i++) {
                if (this.claimsSeleccionados[i].id == claim.id) {
                    this.claimsSeleccionados.splice(i, 1);
                    break;
                }
            }
        }

        crearNuevoUsuario(): void {
            var nombre = this.usuario.nombreDeUsuario;
            this.setWorkingText();
            this.bloquearSubmit = true;

            this.usuario.claims = this.claimsSeleccionados.map(c => { return c.id; });

            this.usuariosService.crearNuevoUsuario(this.usuario,
                (value) => {
                    this.toasterLite.success('El usuario ' + nombre + ' fue creado exitosamente');
                    this.$mdDialog.hide(this.usuario);
                },
                (reason) => {
                    this.setDefaultSubmitText();
                    this.bloquearSubmit = false;
                    this.toasterLite.error('Ocurrió un error y no se pudo crear el usuario');
                });
        }

        //****************************
        // Interfal stuff
        //****************************

        private obtenerListaDeClaims() {
            this.usuariosQueryService.obtenerListaDeClaims(
                response =>
                {
                    this.claimsLoaded = true;
                    this.claims = response.data;
                },
                reason => { this.toasterLite.error('Ocurrió un error al recuperar lista de claims', this.toasterLite.delayForever); });
        }

        private setDefaultSubmitText() {
            this.submitLabel = 'Crear nuevo usuario';
        }

        private setWorkingText() {
            this.submitLabel = 'Creando nuevo usuario...'
        }
    }
}