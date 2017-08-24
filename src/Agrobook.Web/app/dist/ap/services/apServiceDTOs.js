/// <reference path="../../_all.ts" />
var apArea;
(function (apArea) {
    //---------------------
    // DTOS
    //---------------------
    var cliente = (function () {
        function cliente(id, // coopchorti / davidelias
            nombre, // Cooperativa Chortizer / David Elias
            desc, // Loma Plata / Productor de Chooperativa Chortizer y Colonias Unidas
            tipo, // org / prod
            avatarUrl) {
            this.id = id;
            this.nombre = nombre;
            this.desc = desc;
            this.tipo = tipo;
            this.avatarUrl = avatarUrl;
        }
        return cliente;
    }());
    apArea.cliente = cliente;
    var orgDto = (function () {
        function orgDto(id, display) {
            this.id = id;
            this.display = display;
        }
        return orgDto;
    }());
    apArea.orgDto = orgDto;
    var servicioDto = (function () {
        function servicioDto(productorDisplay, fecha, contrato) {
            this.productorDisplay = productorDisplay;
            this.fecha = fecha;
            this.contrato = contrato;
        }
        return servicioDto;
    }());
    apArea.servicioDto = servicioDto;
    var prodDto = (function () {
        function prodDto(id, display) {
            this.id = id;
            this.display = display;
        }
        return prodDto;
    }());
    apArea.prodDto = prodDto;
    /************************
     * Parcela
     ************************/
    var edicionParcelaDto = (function () {
        function edicionParcelaDto() {
        }
        return edicionParcelaDto;
    }());
    apArea.edicionParcelaDto = edicionParcelaDto;
    var parcelaDto = (function () {
        function parcelaDto(id, idProd, display, hectareas, eliminado) {
            if (eliminado === void 0) { eliminado = false; }
            this.id = id;
            this.idProd = idProd;
            this.display = display;
            this.hectareas = hectareas;
            this.eliminado = eliminado;
        }
        return parcelaDto;
    }());
    apArea.parcelaDto = parcelaDto;
    /************************
    * Contrato
    ************************/
    var contratoDto = (function () {
        function contratoDto(id, idOrg, display, esAdenda, eliminado, idContratoDeLaAdenda, fecha) {
            this.id = id;
            this.idOrg = idOrg;
            this.display = display;
            this.esAdenda = esAdenda;
            this.eliminado = eliminado;
            this.idContratoDeLaAdenda = idContratoDeLaAdenda;
            this.fecha = fecha;
        }
        return contratoDto;
    }());
    apArea.contratoDto = contratoDto;
    //-----------------------------------------
    // FakeDb
    //-----------------------------------------
    var fakeDb = (function () {
        function fakeDb() {
            this.fakeClientesList = [
                new cliente("coopchorti", "Cooperativa Chortizer", "Loma Plata", "org", "./assets/img/avatar/org-icon.png"),
                new cliente("ccuu", "Cooperativa Colonias Unidas", "Itapua", "org", "./assets/img/avatar/org-icon.png"),
                new cliente("davidelias", "David Elias", "Cooperativa Chortizer", "prod", "./assets/img/avatar/8.png"),
                new cliente("kazuoyama", "Kazuo Yamazuki", "Cooperativa Pirapo", "prod", "./assets/img/avatar/9.png"),
                new cliente("adair", "Adair Acosta", "Cooperativa Raul Peña", "prod", "./assets/img/avatar/7.png")
            ];
            this.fakeServiciosList = [
                new servicioDto("David Elias", "20/12/2018", "Contrato Chorti"),
                new servicioDto("Kazuo Yamazuki", "20/12/2017", "Contrato Pirapo")
            ];
            // El id de la parcela se puede hacer de la combinacion [prod]_[nombreParcela] debido a que el prod es 
            // igual al usuario, que es unico
            this.parcelas = [
                new parcelaDto('davidelias_DeLaSeñora', 'davidelias', 'De la Señora', '31,66'),
                new parcelaDto('davidelias_Apepu', 'davidelias', 'Apepu', '72,18'),
                new parcelaDto('kazuoyama_Mariscal', 'kazuoyama', 'Mariscal', '73,18'),
                new parcelaDto('kazuoyama_Feliciano', 'kazuoyama', 'Feliciano', '75,18')
            ];
            // Id contrato: [idOrg]_[nombre_contrato]
            // Id adenda: [idContrato]_[nombre_adenda]
            this.contratos = [
                new contratoDto('coopchorti_Contrato Chorti', 'coopchorti', 'Contrato Chorti', false, false, null, new Date(2017, 1, 17)),
                new contratoDto('coopchorti_Contrato Chorti_Adenda I', 'coopchorti', 'Adenda I', true, false, 'Contrato_Chorti', new Date(2017, 2, 20))
            ];
        }
        return fakeDb;
    }());
    fakeDb.$inject = [];
    apArea.fakeDb = fakeDb;
})(apArea || (apArea = {}));
//# sourceMappingURL=apServiceDTOs.js.map