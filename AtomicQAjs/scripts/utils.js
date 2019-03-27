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