/// <reference path="../_all.ts" />
var archivosArea;
(function (archivosArea) {
    var productorDto = (function () {
        function productorDto(id, display, avatarUrl) {
            this.id = id;
            this.display = display;
            this.avatarUrl = avatarUrl;
        }
        return productorDto;
    }());
    archivosArea.productorDto = productorDto;
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