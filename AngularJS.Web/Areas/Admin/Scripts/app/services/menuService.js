'use strict';

app
    .constant('menu.url', {
        getMenus: 'api/admin/Menu',
        postMenu: 'api/admin/Menu'
    })
    .factory('menuApi', ['menu.url', 'core.service',
        function (menuUrl, coreSvc) {

    }
]);