'use strict';
app.controller('loginController', function ($scope, $rootScope, $location, authService, AUTH_EVENTS) {
    $scope.credentials = { userName: '', password: '' };
    $scope.message = '';

    $scope.login = function () {
        if ($scope.userName == '' || $scope.password == '') {
            $scope.message = 'User Name and Password are required!';
            return;
        }

        authService.login($scope.credentials).then(
            function (data) {
                $scope.message = "Login success!";
                $rootScope.$broadcast(AUTH_EVENTS.loginSuccess);
                // if ($location.path().indexOf('login') != -1) 
                    $location.path('/');
                // else 
            }, function (err) {
                $scope.message = "Login failed: " + err.error_description;
            });
    };

});