'use strict';

app.controller('claimUpdateController', ['$scope', '$routeParams', '$location', 'claim.api', 'category.api', 'file.api', 
    'productLine.api', 'vendor.api', 'ngToast', 'dialogs', 'DTOptionsBuilder', 'DTColumnDefBuilder',
function ($scope, $routeParams, $location, claimApi, catApi, fileApi, proApi, vendorApi, ngToast, dialogs,
            DTOptionsBuilder, DTColumnDefBuilder) {
        var dateFormat = "YYYY-MM-DD";

        $scope.title = "Claim Management";
        $scope.claim = { statusID: 0 };
        $scope.options = {};
        $scope.uploadFiles = []; // keep the files while it uploading
        $scope.tab = { active: 0 };

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
            //catApi.getCategories({ type: 'UNIT' }).then(function (data) {
            //    $scope.options.units = data;
            //}, function (error) {
            //    ngToast.create('Error due loading UNIT!');
            //});

            //catApi.getCategories({ type: 'PARTICIPANT' }).then(function (data) {
            //    $scope.options.participants = data;
            //}, function (error) {
            //    ngToast.create('Error due loading PARTICIPANT!');
            //});

            //catApi.getCategories({ type: 'PAYMENTMETHOD' }).then(function (data) {
            //    $scope.options.paymentMethods = data;
            //}, function (error) {
            //    ngToast.create('Error due loading PAYMENTMETHOD!');
            //});

            //catApi.getCategories({ type: 'PROGRAMTYPE' }).then(function (data) {
            //    $scope.options.programTypes = data;
            //}, function (error) {
            //    ngToast.create('Error due loading PROGRAMTYPE!');
            //});

            catApi.getCategories({ type: 'UNIT,PARTICIPANT,PAYMENTMETHOD,PROGRAMTYPE' }).then(function (data) {
                $scope.options.units = [];
                $scope.options.participants = [];
                $scope.options.paymentMethods = [];
                $scope.options.programTypes = [];

                for (var i = 0; i < data.length; i++) {
                    switch (data[i].type) {
                        case 'UNIT': $scope.options.units.push(data[i]); break;
                        case 'PARTICIPANT': $scope.options.participants.push(data[i]); break;
                        case 'PAYMENTMETHOD': $scope.options.paymentMethods.push(data[i]); break;
                        case 'PROGRAMTYPE': $scope.options.programTypes.push(data[i]); break;
                    }
                }

            }, function (error) {
                ngToast.create('Error due loading Unit, Participant, PaymentMethod or ProgramType!');
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

            // Need to load BU, Participant, Unit, Vendor, Program Type, Payment, ProductLine list
            $scope.bus = ['F9', 'F5', 'FAP', 'FHP'];

            var claimId = $routeParams.claimId;
            if (claimId !== undefined && claimId !== 0) {
                $scope.load(claimId);
            } else {
                $scope.actions = {};
                $scope.actions.create = true;
            }
        };

        $scope.load = function (claimId) {
            claimApi.getClaim(claimId).then(function (data) {
                $scope.claim = data;
                $scope.tab.active = $scope.claim.statusID < 15 ? 0 : 1; // Prepare Claim toward
                $scope.objectConfig = data.objectConfig;
                $scope.objectAction = angular.fromJson(data.objectAction);
                $scope.actions = {};
                angular.forEach($scope.objectAction, function (obj) {
                    $scope.actions[obj.toLowerCase()] = true;
                });
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

        // Add/Remove Payment
		var _editPayment = function (pm) {
		    var modal = dialogs.create('dialogs/payment.html?bust=' + Math.random().toString(36).slice(2), 'paymentCtrl', pm);
		    modal.result.then(function (data) {
		        if (data.paymentID == 0) $scope.claim.payments.push(data);
		        else {
		            for (var i = 0; i < $scope.claim.payments.length; i++) {
		                console.log($scope.claim.payments[i]);
		                if ($scope.claim.payments[i].paymentID = data.paymentID) {
		                    console.log('hehe');
		                    $scope.claim.payments[i] = data;
		                    break;
		                }
		            }
		        }
		    }, function (err) {
		    });
		};
		var _removePayment = function (pm) {

		};

        // Add/Remove Allocation
		var _editAllocation = function (aloc) {
		    var modal = dialogs.create('dialogs/allocation.html?bust=' + Math.random().toString(36).slice(2), 'allocationCtrl',
                { aloc: aloc, pms: $scope.claim.payments, alocs: $scope.claim.allocations });
		    modal.result.then(function(data) {
		        if (data.allocationID == 0) $scope.claim.allocations.push(data);
		        else {
		            for (var i = 0; i < $scope.claim.allocations.length; i++) {
		                if ($scope.claim.allocations[i].allocationID == data.allocationID) {
		                    $scope.claim.allocations[i] = data;
		                    break;
		                }
		            }
		        }
		    }, function(err) {

		    });
		};
		var _removeAllocation = function (aloc) {

		};

        // Create new Claim
		var _createClaim = function () {
            // Create new and redirect to this
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
		        $location.path('/claim/' + $scope.claim.claimID);
		    });
		};

		var _submitClaim = function () {
		    claimApi.saveClaim($scope.claim, 'Submit').then(function (data) {
		        ngToast.create('Submitted');
                $location.path('/claim/' + $scope.claim.claimID);
		    });
		};

		var _approveClaim = function () {
		    claimApi.saveClaim($scope.claim, 'Approve').then(function (data) {
		        ngToast.create('Approved');
		        $location.path('/claim/' + $scope.claim.claimID);
		    });
		};

		var _denyClaim = function () {
		    claimApi.saveClaim($scope.claim, 'Deny').then(function (data) {
		        ngToast.create('Denied');
		        $location.path('/claim/' + $scope.claim.claimID);
		    });
		};

        // Upload File
		var _fileSelected = function ($files, $event) {
		    if ($scope.claim.documents == undefined) $scope.claim.documents = [];

		    for (var i = 0; i < $files.length; i++) {
		        (function (index) {
		            var file = $files[i];
		            var newFile = { fileName: file.name, fileType: '', progress: 0, status: 'Uploading' };

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
		};
		var _removeFile = function (file) {
		    var idx = $scope.claim.documents.indexOf(file);
		    $scope.claim.documents.splice(idx, 1);
		};

        // Auto generate checkpoints due customer requirements
		var _createCheckpoint = function () {
		    if ($scope.claim.startDate === undefined || $scope.claim.endDate === undefined) {
		        ngToast.create("Please input Start Date and End Date first (Start Date must before End Date)!");
		    } else {
		        $scope.claim.checkPoints = [];
		        var separate = 4;
		        var _start = moment($scope.claim.startDate);
		        var _end = moment($scope.claim.endDate);
		        var _deadline = moment($scope.claim.deadlineClaimDate);
		        var days = _end.diff(_start, "days");

		        for (var i = 1; i < separate; i++) {
		            if (i != separate && (separate - i) / separate * days <= 15) {
		                continue;
		            }
		            var cpDate = moment(_start).add(Math.round(i / separate), "days");
		            var cp = { action: 'Update định kì!', checkDate: moment(_start).add(i * days / separate, "days").format(dateFormat) };
		            $scope.claim.checkPoints.push(cp);
		        }

		        if (days > 15) {
		            var cp = { action: 'Update định kì (trước khi kết thúc chương trình 15 ngày).', checkDate: moment(_end).add(-15, "days").format(dateFormat) };
		            $scope.claim.checkPoints.push(cp);
		        }

		        if (_deadline.diff(_end, "days") > 15) {
		            var cp = { action: 'Tổng kết chương trình (sau khi kết thúc chương trình 15 ngày).', checkDate: moment(_end).add(15, "days").format(dateFormat) };
		            $scope.claim.checkPoints.push(cp);
		        }

		        console.log($scope.claim.checkPoints);
		    }
		};

        // Mapping functions, run init
        $scope.init = _init;
		$scope.addCheckPoint = _addCP;
		$scope.removeCheckPoint = _removeCP;
		$scope.addRequirement = _addReq;
		$scope.removeRequirement = _removeReq;
		$scope.editPayment = _editPayment;
		$scope.removePayment = _removePayment;
		$scope.editAllocation = _editAllocation;
		$scope.removeAllocation = _removeAllocation;
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
}])
.controller('paymentCtrl', function ($scope, $modalInstance, data) {
    if (data === undefined) $scope.pm = { paymentID: 0 };
    else $scope.pm = data;
    
    $scope.cancel = function () {
        $modalInstance.dismiss('canceled');
    }; // end cancel

    $scope.save = function () {
        // TODO: take a check from server/client with remain payment
        $modalInstance.close($scope.pm);
    }; // end save

    $scope.hitEnter = function (evt) {
        if (angular.equals(evt.keyCode, 13) && !(angular.equals($scope.name, null) || angular.equals($scope.name, '')))
            $scope.save();
    }; // end hitEnter
})
.controller('allocationCtrl', ['$scope', '$modalInstance', 'data', 'category.api',
        function ($scope, $modalInstance, data, catApi) {
            $scope.opt = {};
            $scope.init = function () {
                catApi.getCategories({ type: 'CRITERIA,AREA,PARTICIPANT' }).then(function (data) {
                    $scope.opt.criterias = [];
                    $scope.opt.areas = [];
                    $scope.opt.participants = [];

                    for (var i = 0; i < data.length; i++) {
                        switch (data[i].type) {
                            case 'CRITERIA': $scope.opt.criterias.push(data[i].value); break;
                            case 'AREA': $scope.opt.areas.push(data[i].value); break;
                            case 'PARTICIPANT': $scope.opt.participants.push(data[i].value); break;
                        }
                    }
                }, function (error) {
                });
            };

            if (data.aloc === undefined) $scope.aloc = { allocationID: 0, paymentID: 0 };
            else $scope.aloc = data.aloc;
    
            $scope.pms = data.pms;
            $scope.alocs = data.alocs;
            $scope.remainAloc = 0;
    
            $scope.updateRemainAloc = function(paymentID) {
                $scope.aloc.invoiceCode = '';
                if (paymentID == 0 || paymentID === undefined || paymentID == null) {
                    $scope.remainAloc = 0;
                    return;
                }
        
                var _total = 0;
                for (var _i = 0; _i < $scope.pms.length; _i++) {
                    if ($scope.pms[_i].paymentID == paymentID) {
                        _total = $scope.pms[_i].exchangeRate * $scope.pms[_i].vendorPayment;
                        $scope.aloc.invoiceCode = $scope.pms[_i].invoiceCode;
                    }
                }
        
                for (var _j = 0; _j < $scope.alocs.length; _j++) {
                    if ($scope.alocs[_j].paymentID == paymentID) _total -= $scope.alocs[_j].allocateAmount;
                }
        
                $scope.remainAloc = _total;
            };
    
            $scope.cancel = function () {
                $modalInstance.dismiss('canceled');
            }; // end cancel

            $scope.save = function () {
                // TODO: take a check from server/client with remain payment
                $modalInstance.close($scope.aloc);
            }; // end save

            $scope.hitEnter = function (evt) {
                if (angular.equals(evt.keyCode, 13) && !(angular.equals($scope.name, null) || angular.equals($scope.name, '')))
                    $scope.save();
            }; // end hitEnter
    
            $scope.updateRemainAloc($scope.aloc.paymentID);
            $scope.init();
}])
;