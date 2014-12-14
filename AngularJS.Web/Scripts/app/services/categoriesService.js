'use strict';

app
	.constant('category.url', {
	    getCategories: 'api/Category',
	    getCategory: 'api/Category/'
	})
	.factory('category.api', ['category.url', 'coreSvc',
	function (url, coreSvc) {
	    var api = {
	        getCategories: function (type) {
	            return coreSvc.callApi('GET', url.getCategories, type, null);
	        },
	        getCategory: function (id) {
	            return coreSvc.callApi('GET', url.getCategory, { id: id }, null);
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