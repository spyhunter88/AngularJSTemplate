'use strict';
app.controller('indexController', function ($scope, $location, menuApi, authService) {
	//$scope.navbar = [];
	//$scope.navbar.push({ href: '#/', title: 'Home', isActive: true });
	//$scope.navbar.push({ href: '#/claim', title: 'Claim', isActive: false });
	//$scope.navbar.push({ href: '#/request', title: 'Request', isActive: false });

    // $scope.menus = [];

    // Load menu from database
    menuApi.getMenus().then(
        function (data) {

        }, function (err) {

        });

	$scope.menus = [
        { href: '#/', title: 'Home', route: '/' },
        { href: '#/customer', title: 'Customer', route: '/customer' },
        { href: '#/claim', title: 'Claim', route: '(/claim)|(/newClaim)', submenus: [] },
        {
            href: '#/request', title: 'Request', route: '/request', submenus: [
                { href: '#/newRequest', title: 'New Request' },
                { href: '#/removeRequest', title: 'Remove Request' }
            ]
        }
	];
	
    $scope.logout = function () {
        authService.logOut();
        $location.path('/home');
    }
	
    // Get authentication information to show in menu
	$scope.authentication = authService.authentication;
});