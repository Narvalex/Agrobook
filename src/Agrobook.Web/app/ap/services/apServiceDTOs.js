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
        function orgDto(id, display, avatarUrl) {
            this.id = id;
            this.display = display;
            this.avatarUrl = avatarUrl;
        }
        return orgDto;
    }());
    apArea.orgDto = orgDto;
    var servicioDto = (function () {
        function servicioDto(id, idContrato, esAdenda, idContratoDeLaAdenda, contratoDisplay, idOrg, orgDisplay, idProd, prodDislplay, fecha, observaciones, 
            // With Defaults
            eliminado, 
            // Parcela
            parcelaId, parcelaDisplay, hectareas, 
            // Precio
            tienePrecio, precioTotal, precioPorHectarea) {
            if (eliminado === void 0) { eliminado = false; }
            if (parcelaId === void 0) { parcelaId = null; }
            if (parcelaDisplay === void 0) { parcelaDisplay = null; }
            if (hectareas === void 0) { hectareas = null; }
            if (tienePrecio === void 0) { tienePrecio = false; }
            if (precioTotal === void 0) { precioTotal = null; }
            if (precioPorHectarea === void 0) { precioPorHectarea = null; }
            this.id = id;
            this.idContrato = idContrato;
            this.esAdenda = esAdenda;
            this.idContratoDeLaAdenda = idContratoDeLaAdenda;
            this.contratoDisplay = contratoDisplay;
            this.idOrg = idOrg;
            this.orgDisplay = orgDisplay;
            this.idProd = idProd;
            this.prodDislplay = prodDislplay;
            this.fecha = fecha;
            this.observaciones = observaciones;
            this.eliminado = eliminado;
            this.parcelaId = parcelaId;
            this.parcelaDisplay = parcelaDisplay;
            this.hectareas = hectareas;
            this.tienePrecio = tienePrecio;
            this.precioTotal = precioTotal;
            this.precioPorHectarea = precioPorHectarea;
        }
        return servicioDto;
    }());
    apArea.servicioDto = servicioDto;
    var prodDto = (function () {
        function prodDto(id, display, avatarUrl, orgs) {
            this.id = id;
            this.display = display;
            this.avatarUrl = avatarUrl;
            this.orgs = orgs;
        }
        return prodDto;
    }());
    apArea.prodDto = prodDto;
    var orgConContratos = (function () {
        function orgConContratos(org, contratos) {
            this.org = org;
            this.contratos = contratos;
        }
        return orgConContratos;
    }());
    apArea.orgConContratos = orgConContratos;
    var servicioParaDashboardDto = (function () {
        function servicioParaDashboardDto(id, idProd, prodDisplay, prodAvatarUrl, orgDisplay, parcelaDisplay, fecha) {
            this.id = id;
            this.idProd = idProd;
            this.prodDisplay = prodDisplay;
            this.prodAvatarUrl = prodAvatarUrl;
            this.orgDisplay = orgDisplay;
            this.parcelaDisplay = parcelaDisplay;
            this.fecha = fecha;
        }
        return servicioParaDashboardDto;
    }());
    apArea.servicioParaDashboardDto = servicioParaDashboardDto;
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
        function parcelaDto(id, idProd, display, hectareas, idDepartamento, departamentoDisplay, idDistrito, distritoDisplay, eliminado) {
            if (eliminado === void 0) { eliminado = false; }
            this.id = id;
            this.idProd = idProd;
            this.display = display;
            this.hectareas = hectareas;
            this.idDepartamento = idDepartamento;
            this.departamentoDisplay = departamentoDisplay;
            this.idDistrito = idDistrito;
            this.distritoDisplay = distritoDisplay;
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
    /************************
    * Distritos
    ************************/
    var departamento = (function () {
        function departamento(id, display, distritos) {
            this.id = id;
            this.display = display;
            this.distritos = distritos;
        }
        return departamento;
    }());
    apArea.departamento = departamento;
    var distrito = (function () {
        function distrito(id, display) {
            this.id = id;
            this.display = display;
        }
        return distrito;
    }());
    apArea.distrito = distrito;
    /***************************
    * GROUP BY
    ****************************/
    var contratoConServicios = (function () {
        function contratoConServicios(id, display, totalHa, fecha, eliminado, servicios, 
            // Exclusivo del front end
            colapsado) {
            this.id = id;
            this.display = display;
            this.totalHa = totalHa;
            this.fecha = fecha;
            this.eliminado = eliminado;
            this.servicios = servicios;
            this.colapsado = colapsado;
        }
        return contratoConServicios;
    }());
    apArea.contratoConServicios = contratoConServicios;
    var servicioSlim = (function () {
        function servicioSlim(id, display, eliminado, hectareas, fecha, idProd) {
            this.id = id;
            this.display = display;
            this.eliminado = eliminado;
            this.hectareas = hectareas;
            this.fecha = fecha;
            this.idProd = idProd;
        }
        return servicioSlim;
    }());
    apArea.servicioSlim = servicioSlim;
})(apArea || (apArea = {}));
//# sourceMappingURL=apServiceDTOs.js.map