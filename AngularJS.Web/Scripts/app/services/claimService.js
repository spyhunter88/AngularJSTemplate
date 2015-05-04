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
				return coreSvc.callApi('GET', url.getClaim, { id: id }, null);
			},
			createClaim: function(claim) {
			    return coreSvc.callApi('POST', url.postClaim, null, claim);
			},
			saveClaim: function (claim, action) {
			    return coreSvc.callApi('POST', url.postClaim, { action: action }, claim);
			},
			putClaim: function (claim, action) {
			    return coreSvc.callApi('PUT', url.postClaim, { action: action }, claim);
			}
		};

		return api;
}]);