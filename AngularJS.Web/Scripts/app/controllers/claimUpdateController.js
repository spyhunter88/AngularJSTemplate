﻿'use strict';

app.controller('claimUpdateController', ['$scope', '$routeParams', '$location', 'claim.api', 'category.api', 'file.api', 
    'productLine.api', 'vendor.api', 'ngToast', 'DTOptionsBuilder', 'DTColumnDefBuilder',
function ($scope, $routeParams, $location, claimApi, catApi, fileApi, proApi, vendorApi, ngToast,
            DTOptionsBuilder, DTColumnDefBuilder) {
        $scope.title = "Claim Management";
        // $scope.objectConfig = '{"disable": [ "ftgProgramCode", "programType" ]}';
        $scope.claim = {};
        $scope.options = {};
        $scope.uploadFiles = []; // keep the files while it uploading

        var dtCheckPoint = {};
        dtCheckPoint.options = DTOptionsBuilder.newOptions()
            // .withDisplayLength(5)
            .withDOM('trip')
            .withScroller()
            // .withOptions('deferRender', true)
            .withOption('scrollY', 400)
            .withOption('sort', false)
            .withBootstrap();
        dtCheckPoint.columnDefs = [
            DTColumnDefBuilder.newColumnDef(0).notSortable(),
            DTColumnDefBuilder.newColumnDef(1).notSortable(),
            DTColumnDefBuilder.newColumnDef(2).notSortable(),
            DTColumnDefBuilder.newColumnDef(3).notSortable()
        ];

        var _init = function () {
            catApi.getCategories({ type: 'UNIT' }).then(function (data) {
                $scope.options.units = data;
            }, function (error) {
                ngToast.create('Error due loading UNIT!');
            });

            catApi.getCategories({ type: 'PARTICIPANT' }).then(function (data) {
                $scope.options.participants = data;
            }, function (error) {
                ngToast.create('Error due loading PARTICIPANT!');
            });

            catApi.getCategories({ type: 'PAYMENTMETHOD' }).then(function (data) {
                $scope.options.paymentMethods = data;
            }, function (error) {
                ngToast.create('Error due loading PAYMENTMETHOD!');
            });

            catApi.getCategories({ type: 'PROGRAMTYPE' }).then(function (data) {
                $scope.options.programTypes = data;
            }, function (error) {
                ngToast.create('Error due loading PROGRAMTYPE!');
            });

            proApi.getProductLines({ type: '' }).then(function (data) {
                $scope.options.productLines = data;
            }, function (error) {
                ngToast.create('Error due loading Product Line!');
            });

            vendorApi.getVendors({ type: '' }).then(function (data) {
                $scope.options.vendors = data;
            }, function (error) {
                ngToast.create('Error due loading Vendor!');
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
                // $scope.objectConfig = data.objectConfig;
                $scope.objectConfig = '{"disable": [ "ftgProgramCode", "programType" ]}';
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
            // save
		    claimApi.createClaim($scope.claim).then(function (data) {
		        if (data.claimID !== undefined && data.claimID !== 0) {
		            $scope.claim = data;
		            $location.path('/claim/' + $scope.claim.claimID);
		        }
            }, function (error) {
                ngToast.create("Error");
            });
		};

		var _deleteClaim = function () {
		    console.log('Delete this claim!');
		};

		var _saveClaim = function () {
		    claimApi.saveClaim($scope.claim, 'Save').then(function (data) {
		        ngToast.create('Saved');
		    });
		};

		var _submitClaim = function () {
		    claimApi.saveClaim($scope.claim, 'Submit').then(function (data) {
		        ngToast.create('Submitted');
		    });
		};

		var _approveClaim = function () {

		};

		var _denyClaim = function () {

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

		var _createCheckpoint = function () {

		};

        $scope.init = _init;
		$scope.addCheckPoint = _addCP;
		$scope.removeCheckPoint = _removeCP;
		$scope.addRequirement = _addReq;
		$scope.removeRequirement = _removeReq;
		$scope.fileSelected = _fileSelected;
		$scope.removeFile = _removeFile;

		$scope.createClaim = _createClaim;
		$scope.deleteClaim = _deleteClaim;
		$scope.saveClaim = _saveClaim;
		$scope.submitClaim = _submitClaim;
		$scope.approveClaim = _approveClaim;
		$scope.denyClaim = _denyClaim;

		$scope.createCheckpoint = _createCheckpoint;

		$scope.dt = dtCheckPoint;
        $scope.init();
        // $timeout(function () { $scope.$apply(function() { $scope.load($routeParams.claimId) }) }, 500);
}]);