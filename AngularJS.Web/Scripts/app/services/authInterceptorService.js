'use strict';
app.factory('authInterceptorService', ['$rootScope','$injector', '$q', '$location', 'localStorageService', 'AUTH_EVENTS',
    function ($rootScope, $injector, $q, $location, localStorageService, AUTH_EVENTS) {

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
        
        if (response.status === 401) {
            var authService = $injector.get('authService');
            var authData = localStorageService.get('authorizationData');
            
            if (authData) {
                if (authData.useRefreshTokens) {
                    $location.path('/refresh');
                    $rootScope.$broadcast(AUTH_EVENTS.refreshingToken, response);
                    return $q.reject(response);
                }
            }

            authService.logOut();
            // $location.path('/login');
        }

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