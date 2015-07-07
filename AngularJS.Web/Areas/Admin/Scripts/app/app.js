﻿var app = angular.module('adminApp', ['ngRoute', 'ngSanitize', 'angular.filter', 'ngToast', 'ui.bootstrap', 'adaptv.adaptStrap', 'dialogs.main', 'checklist-model', 'ui.select']);

app.config(function ($routeProvider) {
    $routeProvider.when("/", {
        controller: 'homeController',
        templateUrl: '/Areas/Admin/Content/Static/Home/index.html'
    });

    $routeProvider.when("/account", {
        controller: 'accountController',
        templateUrl: '/Areas/Admin/Content/Static/Account/index.html'
    });

    $routeProvider.when("/policy", {
        controller: 'policyController',
        templateUrl: '/Areas/Admin/Content/Static/Policy/index.html'
    });

    $routeProvider.otherwise({ redirectTo: "/" });
});