'use strict';

app
    .constant('menu.url', {
        getMenus: 'api/admin/Menu',
        postMenu: 'api/admin/Menu'
    })
    .factory('menuApi', ['menu.url', 'core.service',
	function (menuUrl, coreSvc) {
		var api = {
			getMenus: function() {
				return coreSvc.callApi('GET', url.getMenus, null, null);
			},
			postMenus: function(menus) {
				return coreSvc.callApi('POST', url.postMenu, null, menus);
			}
		};
		
		return api;
    }
]);