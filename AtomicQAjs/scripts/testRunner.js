console.log("here");
exports.testRunner = class TestRunner {
	digestInput(webdriver, input) {
		if (typeof input == "string") {
			return input;
		} else {
			return webdriver.Key[input.key];
		}
	}

	digestLocator(By, locator) {
		switch (locator.type) {
			case utils.LOCATOR_TYPES.NAME:
				return By.name(locator.payload);
			default:
				console.log("LOCATOR TYPE NOT SUPPORTED: ", locator);
				break;
		}
	}
	async runTestRunner(test) {
		var webdriver = require("selenium-webdriver");
		var By = webdriver.By;
		var until = webdriver.until;
		var utils = require("./utils.js");

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
						let input = this.digestInput(
							webdriver,
							step.payload.input
						);
						let locator = this.digestLocator(
							By,
							step.payload.locator
						);
						console.log("input", input);
						console.log("to ", locator);
						console.log(step);
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
};
