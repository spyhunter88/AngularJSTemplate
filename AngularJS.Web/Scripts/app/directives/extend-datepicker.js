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
            var link = directive.link;
            directive.compile = function (tElement, tAttr) {
                
                return function (scope, elem, attr) {
                    // define focus from
                    if (scope.focusSource === undefined) scope.focusSource = '';

                    // run inner directive
                    link.apply(this, arguments);

                    if (attr.disabled || attr.readonly) attr.disabled = true;
                    // scope.isOpen = false;

                    // Remove previous date button due scope.apply() function (this include ul and span tag)
                    var prevAppendBtn = elem.next();
                    // console.log(prevAppendBtn);
                    if (prevAppendBtn.length != 0) {
                        if (prevAppendBtn[0].tagName.toLowerCase() == 'ul') prevAppendBtn.remove();
                    }
                    prevAppendBtn = elem.next();
                    if (prevAppendBtn.length != 0) {
                        if (prevAppendBtn[0].tagName.toLowerCase() == 'span') prevAppendBtn.remove();
                    }

                    // Append new one
                    var appendBtn = angular.element('<span class="input-group-btn"><button '
                    + (attr.disabled ? 'disabled' : '')
                    + ' type="button" class="btn btn-default btn-sm"><i class="glyphicon glyphicon-calendar"></i></button></span>');

                    var openDialog = function (event) {
                        event.stopPropagation();
                        // console.log(event);
                        // console.log(scope);
                        // Check if dialog is not open, but event fire after isOpen change from true -> false
                        // So we check false or not exist for first firing
                        if ((scope.focusSource != 'inner') && !attr.disabled) {
                            scope.$apply(function () {
                                // console.log("This cause bug here: " + scope);
                                scope.isOpen = !attr.disabled;
                            });
                        } else {
                            // reset the focus source for later on
                            scope.focusSource = '';
                        }
                    };

                    // We should deny focus bind that come when close the date-picker dialog
                    scope.$on('datepicker.focus', function () {
                        scope.focusSource = 'inner';
                        // console.log('Hehe', scope);
                    });

                    elem.after(appendBtn);
                    appendBtn.on('click', openDialog);

                    elem.on('click', openDialog);
                    elem.on('focus', openDialog);
                };
            };

            return $delegate;
        });
    });
 })(window, angular);