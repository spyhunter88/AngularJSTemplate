'use restrict';

app
	.constant('file.url', {
		fileUrl: 'api/File'
	})
	.factory('file.api', ['file.url', '$upload',
	function(url, $upload) {
		var api = {
			uploadClaimFile: function(claimId, file) {
				var up = $upload.upload({
					url: url.fileUrl,
					method: 'POST',
					data: { claimId: claimId },
					file: file,
					fileFormDataName: file
				});

				return up;
			},
			uploadRequestFile: function(requestId, file) {
				var up = $upload.upload({
				    url: url.fileUrl,
					method: 'POST',
					data: { requestId: requestId },
					file: file
				});

				return up;
			},
			uploadCheckPointFile: function(claimId, checkPointId, file) {
				var up = $upload.upload({
				    url: url.fileUrl,
					method: 'POST',
					data: { claimId: claimId, checkPointId: checkPointId },
					file: file
				});

				return up;
			}
		};

		return api;
	}
]);