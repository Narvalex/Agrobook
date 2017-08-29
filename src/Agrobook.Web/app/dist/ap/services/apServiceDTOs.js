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
        function servicioDto(productorDisplay, fecha, contrato) {
            this.productorDisplay = productorDisplay;
            this.fecha = fecha;
            this.contrato = contrato;
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
            //public servicios = [
            //    new servicioDto("David Elias", "20/12/2018", "Contrato Chorti"),
            //    new servicioDto("Kazuo Yamazuki", "20/12/2017", "Contrato Pirapo")
            //];
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
        }
        return fakeDb;
    }());
    fakeDb.$inject = [];
    apArea.fakeDb = fakeDb;
})(apArea || (apArea = {}));
//# sourceMappingURL=apServiceDTOs.js.map