/// <reference path="../../../_all.ts" />
var apArea;
(function (apArea) {
    var orgTabContratosController = (function () {
        function orgTabContratosController($routeParams, $mdPanel, apQueryService) {
            this.$routeParams = $routeParams;
            this.$mdPanel = $mdPanel;
            this.apQueryService = apQueryService;
            this.submitting = false;
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
        orgTabContratosController.prototype.mostrarOpciones = function ($events, contrato) {
            //let position = this.$mdPanel.newPanelPosition()
            //.relativeTo
        };
        //--------------------------
        // Private
        //--------------------------
        orgTabContratosController.prototype.recuperarContratos = function () {
            var _this = this;
            this.apQueryService.getContratos(this.idOrg, new common.callbackLite(function (value) {
                _this.contratos = value.data;
            }, function (reason) { }));
        };
        orgTabContratosController.prototype.resetForm = function () {
            this.formVisible = false;
            this.dirty = undefined;
            this.submitting = false;
        };
        return orgTabContratosController;
    }());
    orgTabContratosController.$inject = ['$routeParams', '$mdPanel', 'apQueryService'];
    apArea.orgTabContratosController = orgTabContratosController;
})(apArea || (apArea = {}));
//# sourceMappingURL=orgTabContratosController.js.map