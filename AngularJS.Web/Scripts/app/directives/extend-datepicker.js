/*!
 * angular-ext-datepicker - v0.0.1
 * https://github.com/spyhunter88/ext-datepicker
 * License: MIT
 */
 
 (function (window, angular, undefined) {
   'use strict';
	
     angular.module('myExt', ['ui.bootstrap'])
        .directive('datepickerLocaldate', ['$parse', function ($parse) {
            var directive = {
                restrict: 'A',
                require: ['ngModel'],
                link: link
            };
            return directive;
 
            function link(scope, element, attr, ctrls) {
                var ngModelController = ctrls[0];
 
                // called with a JavaScript Date object when picked from the datepicker
                ngModelController.$parsers.push(function (viewValue) {
                    if (viewValue == null) return;
                    var dt = new Date(viewValue);
                    // undo the timezone adjustment we did during the formatting
                    dt.setMinutes(dt.getMinutes() - dt.getTimezoneOffset());
                    // we just want a local date in ISO format
                    return dt.toISOString().substring(0, 10);
                });
 
                // This function helps automatic add TimezoneOffset while server save UTC time, if server has the right time, do not override
                // called with a 'yyyy-mm-dd' string to format
                /*
                ngModelController.$formatters.push(function (modelValue) {
                    if (!modelValue) {
                        return undefined;
                    }
                    // date constructor will apply timezone deviations from UTC (i.e. if locale is behind UTC 'dt' will be one day behind)
                    var dt = new Date(modelValue);
                    // 'undo' the timezone offset again (so we end up on the original date again)
                    dt.setMinutes(dt.getMinutes() + dt.getTimezoneOffset());
                    return dt;
                });
                */
            }
        }])
       .config(function ($provide) {
        $provide.decorator('datepickerPopupDirective', function ($compile, $delegate) {
            var directive = $delegate[0];

            var compile = directive.compile;
            directive.compile = function (tElement, tAttr) {
                var link = compile.apply(this, arguments);

                return function (scope, elem, attr) {
                    if (attr.dateDisabled) attr.disabled = true;

                    var appendBtn = angular.element('<span class="input-group-btn"><button '
                    + (attr.disabled ? 'disabled' : '')
                    + ' type="button" class="btn btn-default btn-sm"><i class="glyphicon glyphicon-calendar"></i></button></span>');
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