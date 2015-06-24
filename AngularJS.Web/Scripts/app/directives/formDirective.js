/*
*   @author: LinhNH13
*/
(function() {
    angular.module('formConfig', []);
    
    /*
    * Input
    */
    angular.module('formConfig').directive('myInput', myInput);
    function myInput($compile) {
        return {
            restrict: 'E',
            scope: {
                ngModel: '=',
                readonly: '='
            },
            require: ['?^form'],
            link: function(scope, element, attrs) {
                // Watch for readonly changed
                scope.$watch('readonly', function(value) {
                    var input = element.find('input');
                    if (value == true) input.attr('readonly', '');
                    else input.removeAttr('readonly');
                });
                
                // Dynamic create input and add to element
                var field = attrs.fieldName;
                var html = '<div class="col-md-6" valdr-form-group>' +
                        '<label class="label-control" for="'+field+'">' + field + '</label>' + 
                        '<input type="text" class="form-control input" name="'+field+'" ng-model="ngModel"' +
                        '/></div>';

                element.html(html);
                $compile(element.contents())(scope);
            }
        };
    };
    
    /*
    * DateInput
    * ---- NOTE ----
    * Remenber readonly can't not be used inside INPUT to bind into datepicker-popup, so we extend date-readonly instead
    */
    angular.module('formConfig').directive('myDateInput', myDateInput);
    function myDateInput($compile) {
        return {
            restrict: 'E',
            scope: {
                ngModel: '=',
                readonly: '='
            },
            require: ['?^form'],
            link: function(scope, element, attrs) {
                scope.$watch('readonly', function(value) {
                    var input = element.find('input');
                    if (value == true) input.attr('readonly', '');
                    else input.removeAttr('readonly');
                });
                
                // We must use date-readonly rather than readonly due conflict with readonly in INPUT element
                var field = attrs.fieldName;
                var html = '<div class="col-md-6" valdr-form-group>' +
                        '<label class="label-control" for="'+field+'">' + field + '</label>' + 
                        '<div class="input-group">' +
                        '<input type="text" class="form-control input" name="'+field+'" ng-model="ngModel"' +
                        'datepicker-popup="yyyy-MM-dd" datepicker-localdate date-readonly="readonly"/>' +
                        '</div></div>';

                element.html(html);
                $compile(element.contents())(scope);
            }
        };
    };
    
    /*
    * SelectInput
    * ---- NOTE ----
    * ngIf creates child scope (while ngShow and ngHide don't), so we must bind to $parent
    */
    angular.module('formConfig').directive('mySelectInput', mySelectInput);
    function mySelectInput($compile) {
        return {
            restrict: 'E',
            scope: {
                ngModel: '=',
                list: '=',
                readonly: '='
            },
            require: ['?^form'],
            link: function(scope, element, attrs) {                
                var field = attrs.fieldName;
                var optStr = attrs.optStr;
                var html = '<div class="col-md-6" valdr-form-group>' +
                        '<label class="label-control" for="'+field+'">' + field + '</label>' +
                        '<input type="text" class="form-control input" ng-model="$parent.ngModel" ng-if="readonly" readonly>' +
                        '<select class="form-control input" ng-if="!readonly" name="'+field+'"' +
                        'ng-model="$parent.ngModel" ng-options="'+optStr+' $parent.list" />' +
                        '</div>';
                
                element.html(html);
                $compile(element.contents())(scope);
            }
        };
    };
    
})();