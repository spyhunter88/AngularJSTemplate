'use strict';
app.factory('authService', ['$http', '$q', 'localStorageService', function ($http, $q, localStorageService) {

    var serviceBase = '';
    var authServiceFactory = {};

    var _authentication = {
        isAuth: false,
        userName: ""
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
        var deferred = $q.defer();

        $http.post(serviceBase + 'token', data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }).success(function (response) {
            localStorageService.set('authorizationData', { token: response.access_token, userName: credentials.userName });
            _authentication.isAuth = true;
            _authentication.userName = credentials.userName;

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

    };

	// Copy authorization state and name that's saved from other session
    var _fillAuthData = function () {
        // TODO: check login success here

        var authData = localStorageService.get('authorizationData');
        if (authData) {
            _authentication.isAuth = true;
            _authentication.userName = authData.userName;
        }

    }

	// expose function
    authServiceFactory.saveRegistration = _saveRegistration;
    authServiceFactory.login = _login;
    authServiceFactory.logOut = _logOut;
    authServiceFactory.fillAuthData = _fillAuthData;
    authServiceFactory.authentication = _authentication;

    return authServiceFactory;
}]);