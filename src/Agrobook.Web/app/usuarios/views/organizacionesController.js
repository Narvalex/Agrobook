/// <reference path="../../_all.ts" />
var usuariosArea;
(function (usuariosArea) {
    var organizacionesController = /** @class */ (function () {
        function organizacionesController(usuariosService, usuariosQueryService, toasterLite, $routeParams, loginQueryService, $rootScope, config, $mdPanel) {
            this.usuariosService = usuariosService;
            this.usuariosQueryService = usuariosQueryService;
            this.toasterLite = toasterLite;
            this.$routeParams = $routeParams;
            this.loginQueryService = loginQueryService;
            this.$rootScope = $rootScope;
            this.config = config;
            this.$mdPanel = $mdPanel;
            // lista de organizaciones
            this.organizaciones = [];
            this.idUsuario = this.$routeParams['idUsuario'];
            if (this.idUsuario === undefined)
                this.idUsuario = this.loginQueryService.tryGetLocalLoginInfo().usuario;
            this.obtenerOrganizaciones();
        }
        organizacionesController.prototype.toggleShowDeleted = function () {
            this.showDeleted = !this.showDeleted;
        };
        organizacionesController.prototype.crearNuevaOrganizacion = function () {
            var _this = this;
            this.creandoOrg = true;
            this.usuariosService.crearNuevaOrganizacion(this.orgNombre, function (value) {
                _this.organizaciones.push(value.data);
                _this.toasterLite.success("La organización " + _this.orgNombre + " fue creada exitosamente");
                _this.creandoOrg = false;
            }, function (reason) {
                _this.toasterLite.error('Ocurrió un error inesperado al intentar crear la organización ' + _this.orgNombre);
                _this.creandoOrg = false;
            });
        };
        organizacionesController.prototype.eliminarOrganizacion = function (org) {
            var _this = this;
            this.creandoOrg = true;
            this.usuariosService.eliminarOrganizacion(org, new common.callbackLite(function (value) {
                for (var i = 0; i < _this.organizaciones.length; i++) {
                    var o = _this.organizaciones[i];
                    if (o.id === org.id) {
                        _this.organizaciones[i].deleted = true;
                        break;
                    }
                }
                _this.creandoOrg = false;
                _this.toasterLite.default('La organización fue eliminada');
            }, function (reason) {
                _this.creandoOrg = false;
                _this.toasterLite.error('Ocurrió un error inesperado al intentar eliminar la organización');
            }));
        };
        organizacionesController.prototype.restaurarOrganizacion = function (org) {
            var _this = this;
            this.creandoOrg = true;
            this.usuariosService.restaurarOrganizacion(org, new common.callbackLite(function (value) {
                for (var i = 0; i < _this.organizaciones.length; i++) {
                    var o = _this.organizaciones[i];
                    if (o.id === org.id) {
                        _this.organizaciones[i].deleted = false;
                        break;
                    }
                }
                _this.creandoOrg = false;
                _this.toasterLite.success('La organización fue restaurada');
            }, function (reason) {
                _this.creandoOrg = false;
                _this.toasterLite.error('Ocurrió un error inesperado al intentar restaurar la organización');
            }));
        };
        organizacionesController.prototype.agregarAOrganizacion = function ($event, org) {
            var _this = this;
            this.agregandoUsuario = true;
            this.usuariosService.agregarUsuarioALaOrganizacion(this.idUsuario, org.id, function (value) {
                // Actualizar la interfaz
                for (var i = 0; i < _this.organizaciones.length; i++) {
                    if (_this.organizaciones[i].id === org.id) {
                        _this.organizaciones[i].usuarioEsMiembro = true;
                        break;
                    }
                }
                _this.$rootScope.$broadcast(_this.config.eventIndex.usuarios.usuarioAgregadoAOrganizacion, {
                    idUsuario: _this.idUsuario,
                    org: org
                });
                _this.toasterLite.success("Usuario agregado exitosamente a " + org.display);
                _this.agregandoUsuario = false;
            }, function (reason) {
                _this.toasterLite.error('Hubo un error al incorporar el usuario a la organizacion', _this.toasterLite.delayForever);
                _this.agregandoUsuario = false;
            });
        };
        organizacionesController.prototype.removerDeLaOrganizacion = function ($event, org) {
            var _this = this;
            this.removiendoUsuario = true;
            this.usuariosService.removerUsuarioDeOrganizacion(this.idUsuario, org.id, new common.callbackLite(function (value) {
                // Actualizar la interfaz
                for (var i = 0; i < _this.organizaciones.length; i++) {
                    if (_this.organizaciones[i].id === org.id) {
                        _this.organizaciones[i].usuarioEsMiembro = false;
                        break;
                    }
                }
                _this.removiendoUsuario = false;
                _this.toasterLite.default('Usuario removido de la organización');
            }, function (reason) {
                _this.removiendoUsuario = false;
                _this.toasterLite.error('Hubo un error al intentar remover usuario de la organización');
            }));
        };
        organizacionesController.prototype.mostrarOpciones = function ($event, org) {
            var position = this.$mdPanel.newPanelPosition()
                .relativeTo($event.srcElement)
                .addPanelPosition(this.$mdPanel.xPosition.ALIGN_START, this.$mdPanel.yPosition.BELOW);
            var config = {
                attachTo: angular.element(document.body),
                controller: panelMenuController,
                controllerAs: 'vm',
                hasBackdrop: true,
                templateUrl: './views/organizaciones-menu-panel.html',
                position: position,
                trapFocus: true,
                locals: {
                    'org': org,
                    'parent': this
                },
                panelClass: 'menu-panel-container',
                openFrom: $event,
                focusOnOpen: true,
                zIndex: 150,
                disableParentScroll: true,
                clickOutsideToClose: true,
                escapeToClose: true,
            };
            this.$mdPanel.open(config);
        };
        //-------------------
        // INTERNAL
        //-------------------
        organizacionesController.prototype.obtenerOrganizaciones = function () {
            var _this = this;
            this.usuariosQueryService.obtenerOrganizacionesMarcadasDelUsuario(this.idUsuario, function (value) {
                _this.organizaciones = value.data;
                _this.loaded = true;
            }, function (reason) { return _this.toasterLite.error('Ocurrió un error al recuperar lista de organizaciones', _this.toasterLite.delayForever); });
        };
        organizacionesController.$inject = ['usuariosService', 'usuariosQueryService', 'toasterLite', '$routeParams', 'loginQueryService', '$rootScope',
            'config', '$mdPanel'];
        return organizacionesController;
    }());
    usuariosArea.organizacionesController = organizacionesController;
    var panelMenuController = /** @class */ (function () {
        function panelMenuController(mdPanelRef, $mdDialog) {
            this.mdPanelRef = mdPanelRef;
            this.$mdDialog = $mdDialog;
        }
        panelMenuController.prototype.removerUsuario = function () {
            var _this = this;
            this.mdPanelRef.close().then(function (value) {
                _this.parent.removerDeLaOrganizacion(null, _this.org);
            })
                .finally(function () { return _this.mdPanelRef.destroy(); });
        };
        panelMenuController.prototype.agregarUsuario = function () {
            var _this = this;
            this.mdPanelRef.close().then(function (value) {
                _this.parent.agregarAOrganizacion(null, _this.org);
            })
                .finally(function () { return _this.mdPanelRef.destroy(); });
        };
        panelMenuController.prototype.cambiarNombre = function ($event) {
            var _this = this;
            this.mdPanelRef.close().then(function (value) {
                var confirm = _this.$mdDialog.prompt()
                    .title('¿Cuál es el nombre correcto del la organización?')
                    .textContent('Escriba el nombre que desea mostrar')
                    .placeholder(_this.org.display)
                    .ariaLabel('Nombre de la org')
                    .initialValue(_this.org.display)
                    .targetEvent($event)
                    .ok('Cambiar nombre')
                    .cancel("Cancelar");
                _this.$mdDialog.show(confirm).then(function (result) {
                    if (_this.org.display === result) {
                        _this.parent.toasterLite.error('¡El nombre propuesto es igual al nombre actual!');
                        return;
                    }
                    _this.parent.usuariosService.cambiarNombreDeOrganizacion(_this.org.id, result, new common.callbackLite(function (value) {
                        // TODO: publicar el cambio.
                        for (var i = 0; i < _this.parent.organizaciones.length; i++) {
                            var org = _this.parent.organizaciones[i];
                            if (org.id === _this.org.id) {
                                _this.parent.organizaciones[i].display = result;
                            }
                        }
                        _this.parent.toasterLite.success('Nombre cambiado exitosamente!');
                    }, function (reason) {
                        _this.parent.toasterLite.error('Hubo un error al intentar cambiar el nombre.');
                    }));
                }, function () { });
            })
                .finally(function () { return _this.mdPanelRef.destroy(); });
        };
        panelMenuController.prototype.eliminarOrg = function () {
            var _this = this;
            this.mdPanelRef.close().then(function (value) {
                _this.parent.eliminarOrganizacion(_this.org);
            })
                .finally(function () { return _this.mdPanelRef.destroy(); });
        };
        panelMenuController.prototype.restaurarOrg = function () {
            var _this = this;
            this.mdPanelRef.close().then(function (value) {
                _this.parent.restaurarOrganizacion(_this.org);
            })
                .finally(function () { return _this.mdPanelRef.destroy(); });
        };
        panelMenuController.prototype.cancelar = function () {
            var _this = this;
            this.mdPanelRef.close().finally(function () { return _this.mdPanelRef.destroy(); });
        };
        panelMenuController.$inject = ['mdPanelRef', '$mdDialog'];
        return panelMenuController;
    }());
})(usuariosArea || (usuariosArea = {}));
//# sourceMappingURL=organizacionesController.js.map