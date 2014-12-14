'use strict';
app.controller('loginController', ['$scope', '$rootScope', '$location', 'AUTH_EVENTS', 'authService', 
    function ($scope, $rootScope, $location, AUTH_EVENTS, authService) {

    $scope.credentials = {
        userName: "",
        password: ""
    };

    $scope.message = "";

    $scope.login = function () {
        console.log('login with: ' + $scope.credentials.userName + ' - ' + $scope.credentials.password)
        authService.login($scope.credentials).then(function (response) {
			$rootScope.$broadcast(AUTH_EVENTS.loginSuccess);
			// $scope.setCurrentUser(response);
            // $location.path('/');
        },
         function (err) {
			$rootScope.$broadcast(AUTH_EVENTS.loginFailed);
			/*
			if (err == null) {
				$scope.message = "Error: Empty response!";
			} else {
				$scope.message = err.error_description;
			}
			*/
         });
    };
}]);