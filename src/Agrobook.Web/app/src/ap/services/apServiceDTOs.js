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
        function servicioDto(id, idContrato, esAdenda, idContratoDeLaAdenda, contratoDisplay, idOrg, orgDisplay, idProd, fecha, 
            // With Defaults
            eliminado, parcelaId, parcelaDisplay) {
            if (eliminado === void 0) { eliminado = false; }
            if (parcelaId === void 0) { parcelaId = null; }
            if (parcelaDisplay === void 0) { parcelaDisplay = null; }
            this.id = id;
            this.idContrato = idContrato;
            this.esAdenda = esAdenda;
            this.idContratoDeLaAdenda = idContratoDeLaAdenda;
            this.contratoDisplay = contratoDisplay;
            this.idOrg = idOrg;
            this.orgDisplay = orgDisplay;
            this.idProd = idProd;
            this.fecha = fecha;
            this.eliminado = eliminado;
            this.parcelaId = parcelaId;
            this.parcelaDisplay = parcelaDisplay;
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
            this.precargar = true;
            // Se genera en otro Bounded Context
            this.orgs = [
                new orgDto('coopchorti', 'Cooperativa Chortizer', './assets/img/avatar/org-icon.png'),
                new orgDto('ccuu', 'Cooperativa Colonias Unidas', './assets/img/avatar/org-icon.png')
            ];
            // Se genera en otro Bounded Context
            this.prods = [
                new prodDto('davidelias', 'David Elías', './assets/img/avatar/8.png', [
                    new orgDto(this.orgs[0].id, this.orgs[0].display, this.orgs[0].avatarUrl)
                ]),
                new prodDto('kazuoyama', 'Kazuo Yamazuki', './assets/img/avatar/9.png', [
                    new orgDto(this.orgs[1].id, this.orgs[1].display, this.orgs[1].avatarUrl)
                ]),
                new prodDto('adair', 'Adair Acosta', './assets/img/avatar/7.png', [
                    new orgDto(this.orgs[0].id, this.orgs[0].display, this.orgs[0].avatarUrl),
                    new orgDto(this.orgs[1].id, this.orgs[1].display, this.orgs[1].avatarUrl)
                ])
            ];
            // El id de la parcela se puede hacer de la combinacion [prod]_[nombreParcela] debido a que el prod es 
            // igual al usuario, que es unico
            this.parcelas = [];
            // Id contrato: [idOrg]_[nombre_contrato]
            // Id adenda: [idContrato]_[nombre_adenda]
            this.contratos = [];
            // Id: Id productor o id org, no serian iguales
            this.clientes = []; // se carga en el constructor
            /**
            * Id: [idProd]_servicio[number + 1]
            */
            this.servicios = [];
            // Cliente preload
            this.clientes = this.orgs.map(function (x) { return new cliente(x.id, x.display, 'Organización', 'org', x.avatarUrl); })
                .concat(this.prods
                .map(function (p) { return new cliente(p.id, p.display, p.orgs.reduce(function (des, org, index, array) {
                if (des === '')
                    des = org.display;
                else
                    des += (', ' + org.display);
                return des;
            }, ''), 'prod', p.avatarUrl); }));
            if (this.precargar) {
                this.parcelas = [
                    new parcelaDto('davidelias_DeLaSeñora', 'davidelias', 'De la Señora', '31,66'),
                    new parcelaDto('davidelias_Apepu', 'davidelias', 'Apepu', '72,18'),
                    new parcelaDto('adair_Mariscal', 'adair', 'Mariscal', '73,18'),
                    new parcelaDto('adair_Feliciano', 'adair', 'Feliciano', '75,18')
                ];
                this.contratos = [
                    new contratoDto('coopchorti_Contrato Chorti', 'coopchorti', 'Contrato Chorti', false, false, null, new Date(2017, 1, 17)),
                    new contratoDto('coopchorti_Contrato Chorti_Adenda I', 'coopchorti', 'Adenda I', true, false, 'Contrato_Chorti', new Date(2017, 2, 20))
                ];
                //this.servicios = [
                //    new servicioDto('adair_servicio1', 'coopchorti_Contrato Chorti', 'Contrato Chorti', 'coopchorti', 'Cooperativa Chortizer', 'adair', new Date(2017, 12, 1)),
                //    new servicioDto('adair_servicio2', 'coopchorti_Contrato Chorti_Adenda I', 'Adenda I', 'coopchorti', 'Cooperativa Chortizer', 'adair', new Date(2017, 12, 30))
                //];
            }
        }
        return fakeDb;
    }());
    fakeDb.$inject = [];
    apArea.fakeDb = fakeDb;
})(apArea || (apArea = {}));
//# sourceMappingURL=apServiceDTOs.js.map