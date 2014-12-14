'use strict';
app.factory('authService', ['$http', '$q', 'localStorageService', function ($http, $q, localStorageService) {

    var serviceBase = '';
    var authServiceFactory = {};

    var _authentication = {
        isAuth: false,
        userName: ""
    };

	// No registration
	/*
    var _saveRegistration = function (registration) {
        _logOut();
        return $http.post(serviceBase + 'api/account/register', registration).then(function (response) {
            return response;
        });
    };
	*/

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

        var authData = localStorageService.get('authorizationData');
        if (authData) {
            _authentication.isAuth = true;
            _authentication.userName = authData.userName;
        }

    }

	// expose function
    // authServiceFactory.saveRegistration = _saveRegistration;
    authServiceFactory.login = _login;
    authServiceFactory.logOut = _logOut;
    authServiceFactory.fillAuthData = _fillAuthData;
    authServiceFactory.authentication = _authentication;

    return authServiceFactory;
}]);