/*!
 * angular-ext-datepicker - v0.0.1
 * https://github.com/spyhunter88/angular-datatables
 * License: MIT
 */
 
 (function (window, angular, undefined) {
   'use strict';
	
	angular
	.module('extUiBootstrap.directives', ['ngToast'])
	.directive('extDatepicker', function ($compile, ngToast) {
		return {
			scope: {
				open: '='
			},
			restrict: 'A',
			replace: false,
			terminal: true, // this setting is important
			priority: 1000, // this setting is important, bigger come first
			controller: function ($scope, $element) {
				$scope.open = function () {
					$scope.preventDefault();
					ngToast.create('Appear');
				};
			},
			link: function (scope, element) {
				scope.open = function () {
					scope.preventDefault();
					ngToast.create('Appear');
				};
			
				var datepicker = '<span class="input-group-btn"><button type="button" class="btn btn-default btn-sm" ng-click="open()"><i class="glyphicon glyphicon-calendar"></i></button></span>';
			
				$compile(datepicker)(scope);
				element.append(datepicker);
			}
		};
	});
 })(window, angular);
 
 (function (window, angular, undefined) {
	'use strict';
  
	angular.module('extUiBootstrap', ['extUiBootstrap.directives'
	]);
  })(window, angular);
/*
app
.directive('extDatepicker', function($compile) {

		/*
		compile: function compile(element, attrs) {
			element.append('<span class="input-group-btn"><button type="button" class="btn btn-default btn-sm" ng-click="open($event)"><i class="glyphicon glyphicon-calendar"></i></button></span>');
			
			return {
				pre: function preLink(scope, iElement, iAttrs, controller) { },
				post: function postLink(scope, iElement, iAttrs, controller) {
					// Must $compile to run ng-click inside new appended element
					// $compile(iElement)(scope);
				}
			};
		}
	};
})
*/