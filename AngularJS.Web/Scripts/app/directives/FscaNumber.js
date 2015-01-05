/*! angular-fsca-number (version 0.0.1) 2015-01-05 */

(function () {
    var fscaNumberModule,
        __hasProp = {}.hasOwnProperty;

    fscaNumberModule = angular.module('fscaNumber', []);

    fscaNumberModule.directive('fscaNumber', [
        'fscaNumberConfig', function (fscaNumberConfig) {
            var addCommasToInteger, controlKeys, defaultOptions, getOptions, hasMultipleDecimals, isNotContrlKey, isNotDigit, isNumber,
                makeIsValid, makeMaxDecimals, makeMaxDigits, makeMaxNumber, makeMinNumber;
            defaultOptions = fscaNumberConfig.defaultOptions;
            getOptions = function (scope) {
                var option, options, value, _ref;
                options = angular.copy(defaultOptions);
                if (scope.options != null) {
                    _ref = scope.$eval(scope.options);
                    for (option in _ref) {
                        if (!__hasProp.call(_ref, option)) continue;
                        value = _ref[option];
                        option[option] = value;
                    }
                }
                return options;
            };

            isNumber = function (val) {
                return !isNaN(parseFloat(val)) && isFinite(val);
            };
            isNotDigit = function (which) {
                return which < 44 || (which > 57 && which < 96) || which > 105 || which === 47;
            };
            isDigitChar = function (which) {
                return which === 8 || which === 173 || which === 188 || which === 190 || which === 110; // "-", "," and "." and "." in num lock
            };
            controlKeys = [0, 8, 13];
            isNotContrlKey = function (which) {
                return controlKeys.indexOf(which) === -1;
            };
            hasMultipleDecimals = function (val) {
                return (val != null) && val.toString().split('.').length > 2;
            };
            makeMaxDecimals = function (val) {
                var regexString, validRegex;
                if (maxDecimals > 0) {
                    regexString = "^-?\\d*\\.?\\d{0," + maxDecimals + "&}";
                } else {
                    regexString = "^-?\\d*$";
                }
                validRegex = new RegExp(regexString);
                return function (val) {
                    return validRegex.test(val);
                };
            };
            makeMaxNumber = function (maxNumber) {
                return function (val, number) {
                    return number <= maxNumber;
                };
            };
            makeMinNumber = function (minNumber) {
                return function (val, number) {
                    return number >= minNumber;
                }
            };
            makeMaxDigits = function (maxDigits) {
                var validRegex;
                validRegex = new RegExp("^-?\\d{0," + maxDigits + "}(\\.\\d*)?$");
                return function (val) {
                    validRegex.test(val);
                };
            };
            makeIsValid = function (options) {
                var validations;
                validations = [];
                if (options.maxDecimals != null) {
                    validations.push(makeMaxDecimal(options.maxDecimals));
                };
                if (options.max != null) {
                    validations.push(makeMaxNumber(options.max));
                };
                if (options.min != null) {
                    validations.push(makeMinNumber(options.min));
                };
                if (options.maxDigits != null) {
                    validations.push(makeMaxDigits(options.maxDigits));
                };
                return function (val) {
                    var i, number, _i, _ref;
                    if (!isNumber(val)) {
                        return false;
                    }
                    if (hasMultipleDecimals(val)) {
                        return false;
                    }
                    number = Number(val);
                    for (i = _i = 0, _ref = validations.length; 0 <= _ref ? _i < _ref : _i > _ref; i < 0 <= _ref ? ++_i : --_i) {
                        if (!validations[i](val, number)) {
                            return false;
                        }
                    }
                    return true;
                };
            };
            addCommasToInteger = function (val) {
                var commas, decimals, wholeNumbers;
                decimals = val.indexOf('.') == -1 ? '' : val.replace(/^-?\d+(?=\.)/, '');
                wholeNumbers = val.replace(/(\.\d+)$/, '');
                commas = wholeNumbers.replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,');
                return "" + commas + decimals;
            };

            return {
                reestrict: 'A',
                require: 'ngModel',
                scope: {
                    options: '@fscaNumber'
                },
                link: function (scope, elem, attrs, ngModelCtrl) {
                    var isValid, options;
                    options = getOptions(scope);
                    isValid = makeIsValid(options);

                    // Parsing value from View to Model when View change
                    ngModelCtrl.$parsers.unshift(function (viewVal) {
                        var noCommasVal;
                        noCommasVal = viewVal.replace(/,/g, '');
                        if (isValid(noCommasVal) || !noCommasVal) {
                            ngModelCtrl.$setValidity('fscaNumber', true);
                            return noCommasVal;
                        } else {
                            ngModelCtrl.$setValidity('fscaNumber', false);
                            return void 0;
                        }
                    });

                    // Formatting value from Model to View when Model change
                    ngModelCtrl.$formatters.push(function (val) {
                        if ((options.nullDisplay != null) && (!val || val === '')) {
                            return options.nullDisplay;
                        }
                        if ((val == null) || !isValid(val)) {
                            return val;
                        }
                        ngModelCtrl.$setValidity('fscaNumber', true);
                        val = addCommasToInteger(val.toString());

                        if (options.prepend != null) {
                            val = "" + options.prepend + val;
                        }
                        if (options.append != null) {
                            val = "" + val + options.append;
                        }
                        return val;
                    });

                    /* Auto parsing - formatting while typing */

                    // Format when lost focus
                    elem.on('blur', function () {
                        var formatter, viewValue, _i, _len, _ref;
                        viewValue = ngModelCtrl.$modelValue;

                        if ((viewValue == null) || !isValid(viewValue)) {
                            return;
                        }
                        _ref = ngModelCtrl.$formatters;
                        for (_i = 0, _len = _ref.length; _i < _len; _i++) {
                            formatter = _ref[_i];
                            viewValue = formatter(viewValue);
                        }
                        ngModelCtrl.$viewValue = viewValue;
                        return ngModelCtrl.$render();
                    });

                    // Remove prepend, append while focus
                    elem.on('focus', function () {
                        var val;
                        val = elem.val();
                        if (options.prepend != null) {
                            val = val.replace(options.prepend, '');
                        }
                        if (options.append != null) {
                            val = val.replace(options.append, '');
                        }
                        // Do not remove commas while focus
                        if (options.formatAsType === undefined || !options.formatAsType) elem.val(val.replace(/,/g, ''));
                        return elem[0].select();
                    });

                    // Re-format while number change, only for commas, decimal'll be formatted on 'blur'
                    elem.on('keyup', function (event) {
                        console.log(options);
                        if (options.formatAsType === undefined || !options.formatAsType) return;
                        if (isNotDigit(event.which) && event.which !== 8 && event.which !== 46) return; // 8 - Backspace, 46 - Delete

                        var val, nonCommasVal;
                        val = elem.val();
                        console.log(event);

                        nonCommasVal = val.replace(/,/g, '');
                        if (nonCommasVal == null || !isValid(nonCommasVal)) return;

                        var formatter, _i, _len, _ref;
                        _ref = ngModelCtrl.$formatters;
                        for (_i = 0, _len = _ref.length; _i < _len; _i++) {
                            formatter = _ref[_i];
                            nonCommasVal = formatter(nonCommasVal);
                        }

                        ngModelCtrl.$viewValue = nonCommasVal;
                        ngModelCtrl.$render();
                    });
                }
            };
        }
    ]);

    fscaNumberModule.provider('fscaNumberConfig', function () {
        var _defaultOptions;
        _defaultOptions = { formatAsType: true };
        this.setDefaultOptions = function (defaultOptions) {
            return _defaultOptions = defaultOptions;
        };
        this.$get = function () {
            return {
                defaultOptions: _defaultOptions
            };
        };
    });

}).call(this);