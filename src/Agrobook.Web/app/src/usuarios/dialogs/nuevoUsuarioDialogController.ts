/// <reference path="../../_all.ts" />

module usuariosArea {
    export class nuevoUsuarioDialogController {

        static $inject = ['$mdDialog', 'usuariosService', 'toasterLite', 'config'];

        constructor(
            private $mdDialog: angular.material.IDialogService,
            private usuariosService: usuariosArea.usuariosService,
            private toasterLite: common.toasterLite,
            private config: common.config
        ) {
            this.setDefaultSubmitText();
            this.avatarUrls = config.avatarUrls;
        }

        avatarUrls = [];

        tiposDeCuenta = [
            { tipo: 'Admin', desc: 'Administrador' },
            { tipo: 'Tecnico', desc: 'Técnico' },
            { tipo: 'Productor', desc: 'Productor' }
        ];

        usuario: UsuarioDto;
        tipoDeCuenta: any;

        bloquearSubmit: boolean = false;
        submitLabel: string;

        cancelar(): void {
            this.$mdDialog.cancel();
        }

        crearNuevoUsuario(): void {
            var nombre = this.usuario.nombreDeUsuario;
            this.setWorkingText();
            this.bloquearSubmit = true;
            this.usuario.claims = [this.tipoDeCuenta.tipo];
            this.usuariosService.crearNuevoUsuario(this.usuario,
                (value) => {
                    this.toasterLite.success('El usuario ' + nombre + ' fue creado exitosamente');
                    this.$mdDialog.hide(this.usuario);
                },
                (reason) => {
                    this.setDefaultSubmitText();
                    this.bloquearSubmit = false;
                    window.alert('Ocurrió un error y no se pudo crear el usuario');
                });
        }

        private setDefaultSubmitText() {
            this.submitLabel = 'Crear nuevo usuario';
        }

        private setWorkingText() {
            this.submitLabel = 'Creando nuevo usuario...'
        }
    }
}