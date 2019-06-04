(function initialize() {
	// Load the main.js file
	require(["client"], function() {
		// on success
		console.log("client.js loaded");
	}, function(error) {
		// on failure
		console.log("error: ", error);
	});

	//------------------------------------------------------------------
	//
	// This function is used to asynchronously load image and audio assets.
	// On success the asset is provided through the onSuccess callback.
	// Reference: http://www.html5rocks.com/en/tutorials/file/xhr2/
	//
	//------------------------------------------------------------------
	function loadAsset(source, onSuccess, onError) {
		let xhr = new XMLHttpRequest();
		let fileExtension = source.substr(source.lastIndexOf(".") + 1); // Source: http://stackoverflow.com/questions/680929/how-to-extract-extension-from-filename-string-in-javascript
		console.log("loading asset", source);
		if (fileExtension) {
			xhr.open("GET", source, true);
			xhr.responseType = "blob";

			xhr.onload = function() {
				console.log("inside onloade", xhr);
				console.log("response text", xhr.responseText);
				let asset = null;
				if (xhr.status === 200) {
					console.log(xhr);
					if (fileExtension === "png" || fileExtension === "jpg") {
						asset = new Image();
					} else if (fileExtension === "mp3") {
						asset = new Audio();
					} else if (fileExtension === "json") {
						asset = {};
					} else {
						if (onError) {
							onError("Unknown file extension: " + fileExtension);
						}
					}
					asset.onload = function() {
						window.URL.revokeObjectURL(asset.src);
					};
					asset.src = window.URL.createObjectURL(xhr.response);
					if (onSuccess) {
						onSuccess(asset);
					}
				} else {
					if (onError) {
						onError("Failed to retrieve: " + source);
					}
				}
			};
		} else {
			if (onError) {
				onError("Unknown file extension: " + fileExtension);
			}
		}

		xhr.send();
	}

	//
	// Load the test.json asset
	loadAsset(
		"/tests/test.json",
		function(test) {
			console.log("test.json loaded");
			console.log("test: ", test);
		},
		function(error) {
			console.log("error: ", error);
		}
	);
})();
