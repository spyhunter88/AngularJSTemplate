'use strict';
app.factory('authInterceptorService', ['$rootScope', '$q', '$location', 'localStorageService', 'AUTH_EVENTS',
    function ($rootScope, $q, $location, localStorageService, AUTH_EVENTS) {

    var authInterceptorServiceFactory = {};

    var _request = function (config) {

        config.headers = config.headers || {};

        var authData = localStorageService.get('authorizationData');
        if (authData) {
            config.headers.Authorization = 'Bearer ' + authData.token;
        }

        return config;
    }

    // broadcast
    var _responseError = function (response) {
        console.log('broadcast response Error: ', response.status);
        $rootScope.$broadcast({
            401: AUTH_EVENTS.notAuthenticated,
            403: AUTH_EVENTS.notAuthorized,
            419: AUTH_EVENTS.sessionTimeout,
            440: AUTH_EVENTS.sessionTimeout
        }[response.status], response);
        return $q.reject(response);
    }

    authInterceptorServiceFactory.request = _request;
    authInterceptorServiceFactory.responseError = _responseError;

    return authInterceptorServiceFactory;
}]);