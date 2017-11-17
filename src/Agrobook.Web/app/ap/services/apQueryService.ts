/// <reference path="../../_all.ts" />

module apArea {
    export class apQueryService extends common.httpLite {
        static $inject = ['$http'];

        constructor(
            private $http: ng.IHttpService,
        ) {
            super($http, '../ap/query');
        }

        getClientes(filtro: string, callback: common.callbackLite<cliente[]>) {
            super.getWithCallback('clientes?filtro=' + filtro, callback);
        }

        getOrg(id: string, callback: common.callbackLite<orgDto>) {
            super.getWithCallback('org/' + id, callback);
        }

        getProd(idProd: string, callback: common.callbackLite<prodDto>) {
            super.getWithCallback('prod/' + idProd, callback);
        }

        getServiciosPorOrg(idOrg: string, callback: common.callbackLite<servicioDto[]>) {
            super.getWithCallback('servicios-por-org/' + idOrg, callback);
        }

        getServiciosPorProd(idProd: string, callback: common.callbackLite<servicioDto[]>) {
            super.getWithCallback('servicios-por-prod/' + idProd, callback);
        }

        getUltimosServicios(cantidad: number, callback: common.callbackLite<servicioParaDashboardDto[]>) {
            super.getWithCallback('ultimos-servicios?cantidad=' + cantidad, callback);
        }

        getServicio(idServicio: string, callback: common.callbackLite<servicioDto>) {
            super.getWithCallback('servicio/' + idServicio, callback);
        }

        getParcelasDelProd(idProd: string, callback: common.callbackLite<parcelaDto[]>) {
            super.getWithCallback('parcelas/' + idProd, callback);
        }

        getParcela(idParcela: string, callback: common.callbackLite<parcelaDto>
        ) {
            super.getWithCallback('parcela/' + idParcela, callback);
        }

        getContratos(idOrg: string, callback: common.callbackLite<contratoDto[]>) {
            super.getWithCallback('contratos/' + idOrg, callback);
        }

        getOrgsConContratosDelProductor(idProd: string, callback: common.callbackLite<orgConContratos[]>) {
            super.getWithCallback('orgs-con-contratos-del-productor/' + idProd, callback);
        }

        getDepartamentos(callback: common.callbackLite<departamento[]>) {
            super.getWithCallback('departamentos', callback);
        }
    }
}

