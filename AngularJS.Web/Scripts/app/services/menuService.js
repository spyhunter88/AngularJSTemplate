/**
* @author: spyhunter88
* currently I just take one level under the root, 
* I'll upgrade to use a recursive function to populate un-limited level
*/

'use strict';

app
	.constant('menu.url', {
	    getMenus: 'api/Menu'
	})
	.factory('menuApi', ['$q', 'menu.url', 'coreSvc',
	function ($q, url, coreSvc) {
	    var api = {
	        getMenus: function () {
	            var deferred = $q.defer();

	            coreSvc.callApi('GET', url.getMenus, null, null).then(
                    function (data) {
                        // processing menu
                        var menu = [];
                        while (data.length > 0) {
                            if (data[0].parentID == 0) {
                                menu.push(data[0]);
                            } else {
                                for (var i = 0; i < menu.length; i++) {
                                    if (menu[i].submenus === undefined) menu[i].submenus = [];
                                    if (data[0].parentID == menu[i].id) menu[i].submenus.push(data[0]);
                                }
                            }
                            data.splice(0,1);
                        }

                        deferred.resolve(menu);
                    }, function (err) {
                        deferred.reject(err);
                    }
                );

	            return deferred.promise;
	        }
	    };

	    return api;
	}]);