'use strict';

app.factory('coreSvc', [
	'$http', '$q', function ($http, $q) {
	    var svc = {
	        sort: function (data) { },
	        filter: function (data) { },
	        callApi: function (_method, _url, _params, _data) {
	            var deferred = $q.defer();

	            $http({ method: _method, url: /* config.apiUrl +*/ _url, params: _params, data: _data }).
					success(function (d, status, headers, config) {
					    deferred.resolve(d);
					}).
					error(function (error) {
					    deferred.reject(error);
					});

	            return deferred.promise;
	        }
	    };

	    return svc;
	}
]);