Main.buttonHandler = () => {
	const Http = new XMLHttpRequest();
	const url = "run-tests";
	Http.open("GET", url);
	Http.send();

	Http.onreadystatechange = e => {
		console.log("server response", Http);
	};
};
