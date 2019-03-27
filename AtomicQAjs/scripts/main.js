async function main (){
	var webdriver = require('selenium-webdriver');
	var By = webdriver.By;
	var until = webdriver.until;

	var driver = new webdriver.Builder()
		.withCapabilities(webdriver.Capabilities.chrome())
		.build()

	var timeout = 10000; // 10 seconds

	async function runGoogleTest(driver) {
		await driver.get('http://www.google.com');
		await inputTextToElement(driver,until, By.name('q'), 'webdriver');
		await inputTextToElement(driver,until, By.name('q'), webdriver.Key.ENTER);
		await titleContainsText(driver, 'webdriver');
	}

	function inputTextToElement(driver, until, by, text) {
		return driver.wait(
				until.elementLocated(by),
				timeout
			).then(element=> {
			element.sendKeys(text);
		})
	}

	function titleContainsText(driver, text) {
		return driver.wait(
			until.titleContains(text),
			timeout
		).then(() => {
			return true;
		}).catch(() => {
			return false;
		})
	}

	await runGoogleTest(driver);
	driver.quit();
};

main();