// "use strict";

let http = require("http");
let path = require("path");
let fs = require("fs");

var utils = require("./scripts/utils.js");

const port = 3000;
var testSteps = [];

// await inputTextToElement(driver, until, By.name("q"), "webdriver");
// await inputTextToElement(driver, until, By.name("q"), webdriver.Key.ENTER);
// await titleContainsText(driver, "webdriver");

function result(test) {
	testSteps = test;
}

utils.loadTest("tests/test.json", result);

async function handleRequest(request, response) {
	let url = request.url;
	console.log("request : ", url);
	console.log("decoded : ", decodeURI(url));

	if (url == "/run-tests") {
		let headers = {
			"Running-Tests": "Test123"
		};
		response.writeHead(200, headers);
		response.end();
		await runTestRunner(testSteps);
	} else {
		let lookup = url === "/" ? "/index.html" : decodeURI(url);
		let file = lookup.substring(1, lookup.length);

		fs.exists(file, exists => {
			console.log(
				exists ? `${lookup} ' is there` : `${lookup} doesn't exist`
			);
			if (exists) {
				fs.readFile(file, function(error, data) {
					if (error) {
						console.log("error...", error);
						response.writeHead(500);
						response.end("Server Error!");
					} else {
						let headers = {
							"Content-type":
								utils.mimeTypes[path.extname(lookup)]
						};
						response.writeHead(200, headers);
					}
				});
			} else {
				response.writeHead(404);
				response.end();
			}
		});
	}
}

http.createServer(handleRequest).listen(port, () => {
	console.log(`server is listening on port ${port}`);
});

function digestInput(webdriver, input) {
	if (typeof input == "string") {
		return input;
	} else {
		return webdriver.Key[input.key];
	}
}

function digestLocator(By, locator) {
	switch (locator.type) {
		case utils.LOCATOR_TYPES.NAME:
			return By.name(locator.payload);
		default:
			console.log("LOCATOR TYPE NOT SUPPORTED: ", locator);
			break;
	}
}

async function runTestRunner(test) {
	var webdriver = require("selenium-webdriver");
	var By = webdriver.By;
	var until = webdriver.until;

	var driver = new webdriver.Builder()
		.withCapabilities(webdriver.Capabilities.chrome())
		.build();

	var timeout = 10000;

	async function runGenericTest(driver, t) {
		for (let i = 0; i < t.length; i++) {
			let step = t[i];
			console.log("step: ", step);
			switch (step.type) {
				case utils.STEP_TYPES.ACT.NAVIGATE:
					console.log("getting", step.payload.URL);
					await driver.get(step.payload.URL);
					break;
				case utils.STEP_TYPES.ACT.INPUT_TEXT_TO_ELEMENT:
					let input = digestInput(webdriver, step.payload.input);
					let locator = digestLocator(By, step.payload.locator);

					console.log("input", input);
					console.log("to ", locator);
					console.log(step);
					// await utils.inputTextToElement(
					// 	driver,
					// 	until,
					// 	By.name(step.payload.locator.payload),
					// 	input
					// );
					await driver
						.wait(until.elementLocated(locator), timeout)
						.then(element => {
							element.sendKeys(input);
						});
					break;
				case utils.STEP_TYPES.ASSERT.TITLE_CONTAINS_TEXT:
					console.log("Title contains", step.payload.text);
					let text = step.payload.text;
					await driver
						.wait(until.titleContains(text), timeout)
						.then(() => {
							return true;
						})
						.catch(() => {
							return false;
						});
					break;
				default:
					console.log("STEP NOT IMPLEMENTED: ", step);
					break;
			}
		}
	}

	async function runGoogleTest(driver) {
		await driver.get("http://www.google.com");
		await utils.inputTextToElement(
			driver,
			until,
			By.name("q"),
			"webdriver"
		);
		await utils.inputTextToElement(
			driver,
			until,
			By.name("q"),
			webdriver.Key.ENTER
		);
		await utils.titleContainsText(driver, "webdriver");
	}

	await runGenericTest(driver, test);
	// await runGoogleTest(driver);
	console.log("QUITTING!");
	driver.quit();
	console.log("DONE QUITTING");
}
