'use strict';
app.controller('indexController', function ($scope, $location, dialogs, menuApi, authService, AUTH_EVENTS) {
	//$scope.navbar = [];
	//$scope.navbar.push({ href: '#/', title: 'Home', isActive: true });
	//$scope.navbar.push({ href: '#/claim', title: 'Claim', isActive: false });
	//$scope.navbar.push({ href: '#/request', title: 'Request', isActive: false });

    $scope.menus = [];

    // Load menu from database include reload authentication from authService
    var _reloadMenu = function () {
        $scope.authentication = authService.authentication;
        menuApi.getMenus().then(
            function (data) {
                // if (data !== undefined && data != '') {
                    // console.log('Start timeout');
                    $scope.menus = data;
                // }
            }, function (err) {

            });
    };
	
    var _logout = function () {
        authService.logOut();
        _reloadMenu();
        $location.path('/home');
    };
    var _login = function () {
        var modal = dialogs.create(
            '/Scripts/app/views/dialogs/loginForm.html?bust=' + Math.random().toString(36).slice(2),
            'loginModalController',
            {}, { size: 'sm' }
            );
        modal.result.then(function (data) { $scope.authentication = authService.authentication; });
    };

    // Run init
    _reloadMenu();
	
    // Get authentication information to show in menu
    $scope.authentication = authService.authentication;
    $scope.login = _login;
    $scope.logout = _logout;

    // register listener
    $scope.$on(AUTH_EVENTS.notAuthenticated, _login);
    $scope.$on(AUTH_EVENTS.notAuthorized, _login);
    $scope.$on(AUTH_EVENTS.sessionTimeout, _login);
    $scope.$on(AUTH_EVENTS.loginSuccess, _reloadMenu);
    // $scope.$on(AUTH_EVENTS.logoutSuccess, _reloadMenu);
})
.controller('loginModalController', function ($scope, $modalInstance, data, $rootScope, $location, authService, AUTH_EVENTS) {
    $scope.credentials = { userName: "", password: "" };
    $scope.message = '';

    // console.log($modalInstance);

    $scope.login = function () {
        authService.login($scope.credentials).then(function (response) {
            $scope.message = "Login success!";

            // return to home if login from main
            if ($location.path().indexOf('login') != -1) $location.path('/');
            else $modalInstance.close({});

            $rootScope.$broadcast(AUTH_EVENTS.loginSuccess);
        }, function (err) {
            $scope.message = "Login failed!";
            // $rootScope.$broadcast(AUTH_EVENTS.loginFailed);
        });
    };
})
;