'use strict';

app.controller('claimUpdateController', ['$scope', '$routeParams', '$timeout', 'claim.api', 'category.api', 'file.api', 'ngToast',
    function ($scope, $routeParams, $timeout, claimApi, catApi, fileApi, ngToast) {
        $scope.title = "Claim Management";
        $scope.options = {};
        $scope.uploadFiles = [];

        var _init = function () {
            catApi.getCategories({ type: 'UNIT' }).then(function (data) {
                $scope.options.units = data;
				ngToast.create('Load UNIT success!');
            }, function (error) {
                ngToast.create('Error due loading UNIT!');
            });
        };

        // Need to load BU, Participant, Unit, Vendor, Program Type, Payment, ProductLine list
        $scope.bus = ['F9', 'F5', 'FAP', 'FHP'];

        $scope.load = function (claimId) {
            claimApi.getClaim(claimId).then(function (data) {
                $scope.claim = data;
                $scope.actions = {};
                $scope.actions.isSave = false;
            }, function(error) {
                alert('Lỗi');
            });
        };
		
		// Add/Remove CheckPoint
        var _addCP = function (cp2Add) {
            if (cp2Add == undefined || cp2Add.$invalid) return;
            if ($scope.claim.checkPoints == undefined) $scope.claim.checkPoints = [];
            cp2Add.id = 0;
            $scope.claim.checkPoints.push(angular.copy(cp2Add));
		};
		var _removeCP = function(cp) {
			var idx = $scope.claim.checkPoints.indexOf(cp);
			$scope.claim.checkPoints.splice(idx, 1);
		};
		
		// Add/Remove Requirement
		var _addReq = function (rq) {
            if (rq == undefined || rq.$invalid) return;
			if ($scope.claim.requirements == undefined) $scope.claim.requirements = [];
			rq.id = 0;
			$scope.claim.requirements.push(angular.copy(rq));
            rq = {};
		};
		var _removeReq = function (rq) {
			var idx = $scope.claim.requirements.indexOf(rq);
			$scope.claim.requirements.splice(idx, 1);
		};

        // Create new Claim
		var _createClaim = function () {
		    // validate first
		    console.log($scope.claim);
            // save
            claimApi.saveClaim($scope.claim).then(function (data) {
                $scope.claim = data;
            }, function (error) {
                alert('1123');
            });
            
		};

        // Upload File
		var _fileSelected = function ($files, $event) {
		    for (var i = 0; i < $files.length; i++) {
		        var file = $files[i];
		        var claimId = 0;
		        if ($scope.claim !== undefined && $scope.claim.claimId !== undefined) claimId = $scope.claim.claimId;
		        fileApi.uploadClaimFile(claimId, file)
                .then(function (data) {
                    console.log(data);
                }, function (error) {
                    console.log(error);
                }, function (evt) {
                    console.log(evt);
                });
		    }
		    console.log($files);
		    console.log($event);
		};

        $scope.init = _init;
		$scope.addCheckPoint = _addCP;
		$scope.removeCheckPoint = _removeCP;
		$scope.addRequirement = _addReq;
		$scope.removeRequirement = _removeReq;
		$scope.createClaim = _createClaim;
		$scope.fileSelected = _fileSelected;
        $scope.init();
        $timeout(function () { $scope.$apply(function() { $scope.load($routeParams.claimId) }) }, 500);
}]);