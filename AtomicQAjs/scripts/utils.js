let path = require("path");
let fs = require("fs");

exports.STEP_TYPES = {
	ACT: {
		NAVIGATE: "navigate",
		INPUT_TEXT_TO_ELEMENT: "input_text_to_element"
	},
	ASSERT: {
		TITLE_CONTAINS_TEXT: "title_contains_text"
	}
};

exports.LOCATOR_TYPES = {
	NAME: "name"
};

var mimeTypes = {
	".js": "text/javascript",
	".html": "text/html",
	".css": "text/css",
	".png": "image/png",
	".jpg": "image/jpeg",
	".mp3": "audio/mpeg3",
	".json": "application/json"
};

exports.mimeTypes = mimeTypes;

exports.loadTest = (filename, result) => {
	fs.exists(filename, exists => {
		if (exists) {
			fs.readFile(filename, function(error, data) {
				console.log("reading...123");
				if (error) {
					console.log("error...123");
				} else {
					result(JSON.parse(data));
				}
			});
		}
	});
};
