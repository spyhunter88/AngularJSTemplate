/*!
 * angular-ext-datepicker - v0.0.1
 * https://github.com/spyhunter88/ext-datepicker
 * License: MIT
 */
 
 (function (window, angular, undefined) {
   'use strict';
	
	angular
	.module('extUiBootstrap.directives', ['ngToast'])
	.directive('extDatepicker', function ($compile, ngToast) {
		return {
			scope: {
			    date: '=ngModel'
			},
			restrict: 'A',
			replace: false,
			terminal: true, // this setting is important
			priority: 1000, // this setting is important, bigger come first
			link: function ($scope, elements, attr) {
			    $scope.openDatePicker = function () {
			        $scope.isOpen = true;
			        console.log('Alright!');
			    };

			    $scope.isOpen = true;
			    elements.attr('datepicker-popup', 'dd/MM/yyyy');
			    elements.attr('is-open', 'isOpen');
			    elements.attr('ng-click', 'isOpen = true;');
			    elements.removeAttr('ext-datepicker');
			    // elements.after('<span class="input-group-btn"><button type="button" class="btn btn-default btn-sm" ng-click="openDatePicker"><i class="glyphicon glyphicon-calendar"></i></button>{{isOpen}}</span>');
			},
			compile: function compile(element, attrs) {
			    return {
			        pre: function preLink(scope, iElement, iAttrs, controller) { },
			        post: function postLink(scope, iElement, iAttrs, controller) {
			            var append = '<span class="input-group-btn"><button type="button" class="btn btn-default btn-sm" ng-click="openDatePicker"><i class="glyphicon glyphicon-calendar"></i></button>{{isOpen}}</span>';
			            $compile(append)(scope);
			            iElement.after(append);
			        }
			    };
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