//app.directive('loginModal', function(AUTH_EVENTS) {
//	return {
//		restrict: 'A',
//		// template: '<div ng-if="visible" ng-include="\'Scripts/app/views/login-form.html\'">',
//		link: function (scope, element) {
//			var showDialog = function() {
//				// scope.visible = true;
//				element.modal("show");
//			};

//			var hideDialog = function() {
//				// scope.visible = false;
//				element.modal("hide");
//			};

//			// scope.visible = false;
//			scope.$on(AUTH_EVENTS.notAuthenticated, showDialog);
//			scope.$on(AUTH_EVENTS.notAuthorized, showDialog);
//			scope.$on(AUTH_EVENTS.sessionTimeout, showDialog);
//			scope.$on(AUTH_EVENTS.loginSuccess, hideDialog);
//		}
//	};
//})