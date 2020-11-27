window.onload = async function () {
  setTimeout(getevents, 100);
};

async function getevents() {
	try {
		await connection.invoke("GetEvents");
	} catch (err) {
		console.error(err);
		window.location.href = "/index.html";
	}
}