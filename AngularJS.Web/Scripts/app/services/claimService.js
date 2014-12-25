'use strict';

app
	.constant('claim.url', {
		getClaims: 'api/Claim',
		getClaim: 'api/Claim/',
        postClaim: 'api/Claim'
	})
	.factory('claim.api', ['claim.url', 'coreSvc',
	function(url, coreSvc) {
		var api = {
			getClaims: function(filterCriteria) {
				return coreSvc.callApi('GET', url.getClaims, filterCriteria, null);
			},
			getClaim: function(id) {
				return coreSvc.callApi('GET', url.getClaim, { id: id}, null);
			},
			saveClaim: function (claim) {
			    return coreSvc.callApi('POST', url.postClaim, null, claim);
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