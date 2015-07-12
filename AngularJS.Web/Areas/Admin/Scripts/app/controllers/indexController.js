'use strict';
app.controller('indexController', function ($scope, $location, menuApi, authService) {
	//$scope.navbar = [];
	//$scope.navbar.push({ href: '#/', title: 'Home', isActive: true });
	//$scope.navbar.push({ href: '#/claim', title: 'Claim', isActive: false });
	//$scope.navbar.push({ href: '#/request', title: 'Request', isActive: false });

    //$scope.userMenu = [];

    //// Load menu from database
    //menuApi.getUserMenus().then(
    //    function (data) {
    //        if (data !== undefined && data != '') $scope.userMenu = data;
    //        // console.log($scope.menus);
    //    }, function (err) {

    //    });

	////$scope.menus = [
    ////    { href: '#/', title: 'Home', route: '/' },
    ////    { href: '#/customer', title: 'Customer', route: '/customer' },
    ////    { href: '#/claim', title: 'Claim', route: '(/claim)|(/newClaim)', submenus: [] },
    ////    {
    ////        href: '#/request', title: 'Request', route: '/request', submenus: [
    ////            { href: '#/newRequest', title: 'New Request' },
    ////            { href: '#/removeRequest', title: 'Remove Request' }
    ////        ]
    ////    }
	////];
	////console.log($scope.menus);
	
    //$scope.logout = function () {
    //    authService.logOut();
    //    $location.path('/home');
    //}

    //// return false if time expire
    //var _isAuth = function () {
    //    var xp = moment(authService.authentication.expireTime).diff(moment(), 's');

    //    if (authService.authentication.isAuth && xp > 0) return true;
    //    else return false;
    //};
	
    //// Get authentication information to show in menu
    //$scope.authentication = authService.authentication;
    //$scope.isAuth = _isAuth;
});