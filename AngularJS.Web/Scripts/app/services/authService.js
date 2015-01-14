'use strict';
app.factory('authService', ['$http', '$q', 'localStorageService', function ($http, $q, localStorageService) {

    var serviceBase = '';
    var clientId = "ngAuthApp";
    var authServiceFactory = {};

    var _authentication = {
        isAuth: false,
        userName: "",
        useRefreshTokens: false
    };

    var _externalAuthData = {
        provider: "",
        userName: "",
        externalAccessToken: ""
    };

    // No registration
    var _saveRegistration = function (registration) {
        _logOut();
        var deferred = $q.defer();

        return $http.post(serviceBase + 'api/Account/Register', registration).then(function (response) {
            deferred.resolve(response);
        }, function (err, status) {
            deferred.reject(err);
        });

        return deferred.promise;
    };

    // Login and get UserName
    var _login = function (credentials) {
        var data = "grant_type=password&username=" + credentials.userName + "&password=" + credentials.password;
        if (credentials.useRefreshTokens) {
            data = data + "&client_id=" + clientId;
        }

        var deferred = $q.defer();

        $http.post(serviceBase + 'token', data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }).success(function (response) {

            if (credentials.useRefreshTokens) {
                localStorageService.set('authorizationData', { token: response.access_token, userName: credentials.userName, refreshToken: response.refresh_token, useRefreshTokens: true });
            } else {
                localStorageService.set('authorizationData', { token: response.access_token, userName: credentials.userName, refreshToken: "", useRefreshToken: false });
            }

            _authentication.isAuth = true;
            _authentication.userName = credentials.userName;
            _authentication.useRefreshTokens = credentials.useRefreshTokens;
            deferred.resolve(response);
        }).error(function (err, status) {
            _logOut();
            deferred.reject(err);
        });

        return deferred.promise;
    };

    // Logout, remove authorization state, userName.
    var _logOut = function () {
        localStorageService.remove('authorizationData');

        _authentication.isAuth = false;
        _authentication.userName = "";
        _authentication.useRefreshTokens = false;
    };

    // Copy authorization state and name that's saved from other session
    var _fillAuthData = function () {
        // TODO: check login success here

        var authData = localStorageService.get('authorizationData');
        if (authData) {
            _authentication.isAuth = true;
            _authentication.userName = authData.userName;
            _authentication.useRefreshTokens = authData.useRefreshTokens;
        }
    }

    var _refreshToken = function () {
        var deferred = $q.defer();
        var authData = localStorageService.get('authorizationData');

        if (authData) {
            var data = "grant_type=refresh_token&refresh_token=" + authData.refreshToken + "&client_id=" + clientId;
            localStorageService.remove('authorizationData');
            $http.post(serviceBase + 'token', data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }).success(function (response) {
                localStorageService.set('authorizationData', { token: response.access_token, userName: response.userName, refreshToken: response.refresh_token, useRefreshTokens: true });
                deferred.resolve(response);
            }).error(function (err, status) {
                _logOut();
                deferred.reject(err);
            });
        }

        return deferred.promise;
    };

    var _obtainAccessToken = function (externalData) {
        var deferred = $q.defer();

        $http.get(serviceBase + 'api/Account/ObtainLocalAccessToken', { params: { provider: externalData.provider, externalAccessToken: externalData.externalAccessToken } }).success(function (response) {
            localStorageService.set('authorizationData', { token: response.access_token, userName: response.userName, refreshToken: "", useRefreshTokens: false });

            _authentication.isAuth = true;
            _authentication.userName = response.userName;
            _authentication.useRefreshTokens = false;

            deferred.resolve(response);
        }).error(function (err, status) {
            _logOut();
            deferred.reject(err);
        });

        return deferred.promise;
    };

    var _registerExternal = function (registerExternalData) {
        var deferred = $q.defer();

        $http.post(serviceBase + 'api/Account/RegisterExternal', registerExternalData).success(function (response) {
            localStorageService.set('authorizationData', { token: response.access_token, userName: response.userName, refreshToken: "", useRefreshTokens: false });

            _authentication.isAuth = true;
            _authentication.userName = response.userName;
            _authentication.useRefreshTokens = false;

            deferred.resolve(response);
        }).error(function (err, status) {
            _logOut();
            deferred.reject(err);
        });

        return deferred.promise;
    };

    // expose function
    authServiceFactory.saveRegistration = _saveRegistration;
    authServiceFactory.login = _login;
    authServiceFactory.logOut = _logOut;
    authServiceFactory.fillAuthData = _fillAuthData;
    authServiceFactory.authentication = _authentication;
    authServiceFactory.refreshToken = _refreshToken;

    authServiceFactory.obtainAccessToken = _obtainAccessToken;
    authServiceFactory.externalAuthData = _externalAuthData;
    authServiceFactory.registerExternal = _registerExternal;

    return authServiceFactory;
}]);