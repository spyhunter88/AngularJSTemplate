'use strict';

app
	.constant('productLine.url', {
	    getProductLines: 'api/ProductLine',
	    getProductLine: 'api/ProductLine/'
	})
	.factory('productLine.api', ['productLine.url', 'coreSvc',
	function (url, coreSvc) {
	    var api = {
	        getProductLines: function (type) {
	            return coreSvc.callApi('GET', url.getProductLines, type, null);
	        },
	        getProductLine: function (id) {
	            return coreSvc.callApi('GET', url.getProductLine, { id: id }, null);
	        }
	    };

	    return api;
	    /*
		return {
		loadClaims: function(filterCriteria) {
			$http
				.get('api/Claims', { param: { filterCriteria: filterCriteria } })
				.success(function(data, status) {
					$scope.claims = data.claims;
					$scope.totalPages = data.total;
			});
		},
		loadClaim: function (claimId) {
		    $http.get('api/Claim?claimId=' + claimId)
                .success(function (data, status) {
                    $scope.claim = data;
                });
		}
		}
		*/
	}]);