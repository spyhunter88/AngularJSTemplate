var app = angular.module('adminApp', ['ngRoute', 'ngSanitize', 'ngToast', 'ui.bootstrap', 'adaptv.adaptStrap', 'dialogs.main']);

app.config(function ($routeProvider) {
    $routeProvider.when("/", {
        controller: 'homeController',
        templateUrl: '/Areas/Admin/Content/Static/Home/index.html'
    });

    $routeProvider.when("/account", {
        controller: 'accountController',
        templateUrl: '/Areas/Admin/Content/Static/Account/index.html'
    });

    $routeProvider.when("/account/:id", {
        controller: 'accountController',
        templateUrl: '/Areas/Admin/Content/Static/Account/edit.html'
    });

    $routeProvider.otherwise({ redirectTo: "/" });
});