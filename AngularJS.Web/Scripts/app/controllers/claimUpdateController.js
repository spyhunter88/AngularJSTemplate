'use strict';

app.controller('claimUpdateController', ['$scope', '$routeParams', '$location', 'claim.api', 'category.api', 'file.api', 'ngToast',
    function ($scope, $routeParams, $location, claimApi, catApi, fileApi, ngToast) {
        $scope.title = "Claim Management";
        $scope.options = {};
        $scope.uploadFiles = []; // keep the files while it uploading

        var _init = function () {
            catApi.getCategories({ type: 'UNIT' }).then(function (data) {
                $scope.options.units = data;
				ngToast.create('Load UNIT success!');
            }, function (error) {
                ngToast.create('Error due loading UNIT!');
            });

            var claimId = $routeParams.claimId;
            if (claimId !== undefined && claimId !== 0) {
                $scope.load(claimId);
            }
        };

        // Need to load BU, Participant, Unit, Vendor, Program Type, Payment, ProductLine list
        $scope.bus = ['F9', 'F5', 'FAP', 'FHP'];

        $scope.load = function (claimId) {
            claimApi.getClaim(claimId).then(function (data) {
                $scope.claim = data;
                $scope.actions = {};
                $scope.actions.isSave = false;
            }, function(error) {
                ngToast.create("Error");
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
		        if (data.claimId !== undefined && data.claimId !== 0) {
		            $scope.claim = data;
		            $location.path('/claim/' + $scope.claim.claimId);
		        }
            }, function (error) {
                ngToast.create("Error");
            });
            
		};

        // Upload File
		var _fileSelected = function ($files, $event) {
		    if ($scope.claim.documents == undefined) $scope.claim.documents = [];

		    for (var i = 0; i < $files.length; i++) {
		        (function (index) {
		            var file = $files[i];
		            var newFile = { fileName: file.name, fileType: 'blank', progress: 0, status: 'Uploading' };

		            var claimId = 0;
		            if ($scope.claim !== undefined && $scope.claim.claimId !== undefined) claimId = $scope.claim.claimId;
		            newFile.fn = fileApi.uploadClaimFile(claimId, file)
                    .then(function (res) {
                        newFile.progress = 100;
                        newFile.tempName = res.data.tempName;
                        newFile.status = 'Completed';
                        console.log(res);
                    }, function (error) {
                        newFile.status = 'Failed';
                        //console.log(error);
                    }, function (evt) {
                        //console.log(evt);
                        newFile.progress = Math.round(evt.loaded / evt.total * 100);
                    });

		            $scope.claim.documents.push(newFile);
		        })(i);
		    }
		    // console.log($files);
		    // console.log($event);
		};
		var _removeFile = function (file) {
		    var idx = $scope.claim.documents.indexOf(file);
		    $scope.claim.documents.splice(idx, 1);
		};

        $scope.init = _init;
		$scope.addCheckPoint = _addCP;
		$scope.removeCheckPoint = _removeCP;
		$scope.addRequirement = _addReq;
		$scope.removeRequirement = _removeReq;
		$scope.createClaim = _createClaim;
		$scope.fileSelected = _fileSelected;
		$scope.removeFile = _removeFile;
        $scope.init();
        // $timeout(function () { $scope.$apply(function() { $scope.load($routeParams.claimId) }) }, 500);
}]);