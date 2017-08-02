/// <reference path="../_all.ts" />

module apArea {
    angular.module('apArea', ['ngRoute', 'ngMaterial', 'ngMdIcons'])
        .value('config', new common.config());
}