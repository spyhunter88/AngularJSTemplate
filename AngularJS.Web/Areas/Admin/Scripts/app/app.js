var app = angular.module('adminApp', ['ngRoute', 'ngSanitize', 'ngAnimate', 'LocalStorageModule', 'angular.filter', 'ngToast', 'ui.bootstrap',
        'adaptv.adaptStrap', 'dialogs.main', 'checklist-model', 'ui.select', 'myMenu']);

app.constant('APP_SETTINGS', {
    Module_Name: 'Admin'
});

app.config(function ($routeProvider) {
    //$routeProvider.when("/", {
    //    controller: 'homeController',
    //    templateUrl: '/Areas/Admin/Content/Static/Home/index.html'
    //});

    $routeProvider.when('/menu', {
        controller: 'menuController',
        templateUrl: '/Areas/Admin/Content/Static/Menu/index.html',
        resolve: { authenticate: function () { return true; } }
    });

    $routeProvider.when("/account", {
        controller: 'accountController',
        templateUrl: '/Areas/Admin/Content/Static/Account/index.html',
        resolve: { authenticate: function () { return true; } }
    });

    $routeProvider.when("/policy", {
        controller: 'policyController',
        templateUrl: '/Areas/Admin/Content/Static/Policy/index.html',
        resolve: { authenticate: function () { return true; } }
    });

    $routeProvider.when("/login", {
        controller: "loginController",
        templateUrl: '/Areas/Admin/Content/Static/Login/index.html'
    });

    $routeProvider.otherwise({ redirectTo: "/" });
});

// push authInterceptor into httpProvider
app.config(function ($httpProvider) {
    $httpProvider.interceptors.push('authInterceptorService');
});

// Load Authentication cookie into authService if exists when Angular/Web page start
app.run(function ($rootScope, AUTH_EVENTS, authService) {
    // First, load authentication that save before
    authService.fillAuthData();

    // inject routeChangeStart to open login or error (unauthorize) dialog
    $rootScope.$on('$routeChangeStart', function (event, cur, prev) {
        var authData = authService.authentication;
        if (!authData.isAuth || moment(authData.expireTime).diff(moment(), 's') < 0) {
            if (cur.$$route && cur.$$route.resolve && cur.$$route.resolve.authenticate) {
                // open login dialog first, the 2.0 version will come with check roles and Un Authorized
                $rootScope.$broadcast(AUTH_EVENTS.notAuthenticated);
            }
        }
    });
});