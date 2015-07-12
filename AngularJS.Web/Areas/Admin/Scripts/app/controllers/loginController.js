'use strict';
app.controller('loginController', ['$scope', '$rootScope', '$location', 'AUTH_EVENTS', 'authService', 
    function ($scope, $rootScope, $location, AUTH_EVENTS, authService) {

        if (authService.isAuthen()) $location.path('/');

        $scope.credentials = {
            userName: "",
            password: "",
            message: ""
        };

        $scope.login = function () {
            authService.login($scope.credentials).then(function (response) {
                $scope.message = "Login success!";
			
                // return to home if login from main
                if ($location.path().indexOf('login') != -1) $location.path('/');
                else $rootScope.$broadcast(AUTH_EVENTS.loginSuccess);
            },function (err) {
                $scope.message = "Login failed!";
			    $rootScope.$broadcast(AUTH_EVENTS.loginFailed);
             });
        };

        $scope.registerData = { userName: '', password: '', confirmPassword: '', message: '' };
        $scope.register = function () {
            authService.saveRegistration($scope.registerData).then(
                function (response) {
                    $scope.registerData.message = 'Register success!';
                }, function (err) {
                    if (err == null) {
                        $scope.registerData.message = 'Error: Empty response!';
                    } else {
                        $scope.registerData.message = 'Error: ' + err.error_description;
                    }
                }
            );
        };
}]);