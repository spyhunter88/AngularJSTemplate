/*!
 * angular-ext-datepicker - v0.0.1
 * https://github.com/spyhunter88/ext-datepicker
 * License: MIT
 */
 
 (function (window, angular, undefined) {
   'use strict';
	
   angular.module('myExt', ['ui.bootstrap'])
       .config(function ($provide) {
        $provide.decorator('datepickerPopupDirective', function ($compile, $delegate) {
            var directive = $delegate[0];

            var compile = directive.compile;
            directive.compile = function (tElement, tAttr) {
                var link = compile.apply(this, arguments);

                return function (scope, elem, attr) {
                    if (attr.dateDisabled) attr.disabled = true;

                    var appendBtn = angular.element('<span class="input-group-btn"><button '
                    + (attr.disabled ? 'disabled' : '') + ' type="button" class="btn btn-default btn-sm"><i class="glyphicon glyphicon-calendar"></i></button></span>');
                    elem.after(appendBtn);
                    appendBtn.bind('click', function (event) {
                        event.stopPropagation();
                        scope.$apply(function () {
                            scope.isOpen = !attr.disabled && true;
                        });
                    });

                    elem.bind('click', function (event) { });

                    link.apply(this, arguments);
                };
            };

            return $delegate;
        });
    });
 })(window, angular);