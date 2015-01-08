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
}]);