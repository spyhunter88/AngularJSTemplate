'use strict';
app.controller('indexController', ['$scope', '$location', 'authService', function ($scope, $location, authService) {
	$scope.navbar = [];
	$scope.navbar.push({ href: '#/', title: 'Home', isActive: true });
	$scope.navbar.push({ href: '#/claim', title: 'Claim', isActive: false });
	$scope.navbar.push({ href: '#/request', title: 'Request', isActive: false });
	
    $scope.logout = function () {
        alert('Should Log out here!');
        authService.logOut();
        $location.path('/home');
    }
	
	$scope.authentication = authService.authentication;
}]);