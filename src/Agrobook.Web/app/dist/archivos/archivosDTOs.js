/// <reference path="../_all.ts" />
var archivosArea;
(function (archivosArea) {
    var productorDto = (function () {
        function productorDto(id, display, avatarUrl, organizaciones) {
            this.id = id;
            this.display = display;
            this.avatarUrl = avatarUrl;
            this.organizaciones = organizaciones;
        }
        return productorDto;
    }());
    archivosArea.productorDto = productorDto;
    var Organizacion = (function () {
        function Organizacion(nombre, grupos) {
            this.nombre = nombre;
            this.grupos = grupos;
        }
        return Organizacion;
    }());
    archivosArea.Organizacion = Organizacion;
    var archivoDto = (function () {
        function archivoDto(nombre, extension, fecha, desc) {
            this.nombre = nombre;
            this.extension = extension;
            this.fecha = fecha;
            this.desc = desc;
        }
        return archivoDto;
    }());
    archivosArea.archivoDto = archivoDto;
})(archivosArea || (archivosArea = {}));
//# sourceMappingURL=archivosDTOs.js.map