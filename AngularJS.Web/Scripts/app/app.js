var app = angular.module('AngularAuthApp', ['kendo.directives', 'ngRoute', 'LocalStorageModule', 'angular-loading-bar', 'ui.bootstrap', 'ngToast', 'extUiBootstrap']);

app.config(function ($routeProvider) {
	$routeProvider.when("/", {
        controller: "dashBoardController",
        templateUrl: "Main/DashBoard"
    });
	
	$routeProvider.when("/login", {
        controller: "loginController",
        templateUrl: "Main/Login"
	});

	$routeProvider.when("/unauthorize", {
	    controller: "unauthorizeController",
        templateUrl: "Main/UnAuthorize"
	});

	$routeProvider.when("/customer", {
	    controller: "customerController",
        templateUrl: "Customer/Index"
	});

	$routeProvider.when("/claim", {
	    controller: "claimController",
	    templateUrl: "Claim/Index"
	});

	$routeProvider.when("/newClaim", {
	    controller: "claimUpdateController",
        templateUrl: "Claim/Create"
	});

	$routeProvider.when("/claim/:claimId", {
	    controller: "claimUpdateController",
        templateUrl: "Claim/Update",
        resolve: {
        	authenticate: function() {return true;}
        }
	});
	
	$routeProvider.when("/request", {
	    controller: "requestController",
	    templateUrl: "Request/Index",
	    resolve: {
        	authenticate: function() {return true;}
        }
	});

	$routeProvider.otherwise({ redirectTo: "/" });
});

// push authInterceptor into httpProvider
app.config(function ($httpProvider) {
    $httpProvider.interceptors.push('authInterceptorService');
});

// Load Authentication cookie into authService if exists when Angular/Web page start
app.run(function ($rootScope, AUTH_EVENTS, authService) {
	// First, load authentication that save before
    authService.fillAuthData();

    // inject routeChangeStart to open login or error (unauthorize) dialog
    $rootScope.$on('$routeChangeStart', function (event, cur, prev) {
    	// var functionName = next.data.functionName;
    	if (!authService.authentication.isAuth) {
	    	if(cur.$$route && cur.$$route.resolve && cur.$$route.resolve.authenticate) {
	    		// event.preventDefault();

	    		console.log('Route changed!');

	    		// open login dialog first, the 2.0 version will come with check roles and Un Authorized
	    		$rootScope.$broadcast(AUTH_EVENTS.notAuthenticated);
    		}
    	}
    });
});