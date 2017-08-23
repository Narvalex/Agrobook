/// <reference path="../../../_all.ts" />
var apArea;
(function (apArea) {
    var orgTabContratosController = (function () {
        function orgTabContratosController($routeParams, $mdPanel, apQueryService, apService) {
            this.$routeParams = $routeParams;
            this.$mdPanel = $mdPanel;
            this.apQueryService = apQueryService;
            this.apService = apService;
            this.submitting = false;
            this.tieneContrato = false;
            this.idOrg = this.$routeParams['idOrg'];
            this.recuperarContratos();
        }
        //--------------------------
        // Api
        //--------------------------
        orgTabContratosController.prototype.mostrarForm = function (editMode) {
            this.editMode = editMode;
            this.formVisible = true;
            setTimeout(function () { return document.getElementById('nombreContratoInput').focus(); }, 0);
        };
        orgTabContratosController.prototype.cancelar = function () {
            this.formVisible = false;
            this.resetForm();
        };
        orgTabContratosController.prototype.mostrarOpciones = function ($event, contrato) {
            var position = this.$mdPanel.newPanelPosition()
                .relativeTo($event.srcElement)
                .addPanelPosition(this.$mdPanel.xPosition.ALIGN_START, this.$mdPanel.yPosition.BELOW);
            var config = {
                attachTo: angular.element(document.body),
                controller: panelMenuController,
                controllerAs: 'vm',
                hasBackdrop: true,
                templateUrl: './dist/ap/views/org/menu-panel-tab-contratos.html',
                position: position,
                trapFocus: true,
                locals: {
                    'contrato': contrato,
                    'parent': this
                },
                panelClass: 'menu-panel-container',
                openFrom: $event,
                focusOnOpen: true,
                zIndex: 150,
                disableParentScroll: true,
                clickOutsideToClose: true,
                escapeToClose: true
            };
            this.$mdPanel.open(config);
        };
        orgTabContratosController.prototype.habilitarEdicion = function (contrato) {
            this.dirty = contrato;
            this.mostrarForm(true);
        };
        orgTabContratosController.prototype.eliminar = function (contrato) {
        };
        orgTabContratosController.prototype.restaurar = function (contrato) {
        };
        orgTabContratosController.prototype.submit = function () {
        };
        //--------------------------
        // Private
        //--------------------------
        orgTabContratosController.prototype.recuperarContratos = function () {
            var _this = this;
            this.apQueryService.getContratos(this.idOrg, new common.callbackLite(function (value) {
                _this.contratos = value.data;
                // Preparando
                _this.soloContratos = _this.contratos.filter(function (x) { return !x.esAdenda; });
                // Si tiene contrato
                if (_this.soloContratos.length > 0) {
                    _this.tieneContrato = true;
                    _this.contratoAdendado = _this.soloContratos[0];
                    _this.tipoContrato = 'adenda';
                }
                else {
                    _this.tipoContrato = 'contrato';
                }
            }, function (reason) { }));
        };
        orgTabContratosController.prototype.resetForm = function () {
            this.formVisible = false;
            this.dirty = undefined;
            this.submitting = false;
        };
        orgTabContratosController.prototype.formatearFecha = function (fecha) {
            return moment(fecha).format('DD/MM/YYYY');
        };
        return orgTabContratosController;
    }());
    orgTabContratosController.$inject = ['$routeParams', '$mdPanel', 'apQueryService', 'apService'];
    apArea.orgTabContratosController = orgTabContratosController;
    var panelMenuController = (function () {
        function panelMenuController(mdPanelRef) {
            this.mdPanelRef = mdPanelRef;
        }
        panelMenuController.prototype.editar = function () {
            var _this = this;
            this.mdPanelRef.close().then(function (value) {
                _this.parent.habilitarEdicion(_this.contrato);
            })
                .finally(function () { return _this.mdPanelRef.destroy(); });
        };
        panelMenuController.prototype.eliminar = function () {
            var _this = this;
            this.mdPanelRef.close().then(function (value) {
                _this.parent.eliminar(_this.contrato);
            })
                .finally(function () { return _this.mdPanelRef.destroy(); });
        };
        panelMenuController.prototype.restaurar = function () {
            var _this = this;
            this.mdPanelRef.close().then(function (value) {
                _this.parent.restaurar(_this.contrato);
            })
                .finally(function () { return _this.mdPanelRef.destroy(); });
        };
        panelMenuController.prototype.cancelar = function () {
            var _this = this;
            this.mdPanelRef.close().finally(function () { return _this.mdPanelRef.destroy(); });
        };
        return panelMenuController;
    }());
    panelMenuController.$inject = ['mdPanelRef'];
})(apArea || (apArea = {}));
//# sourceMappingURL=orgTabContratosController.js.map