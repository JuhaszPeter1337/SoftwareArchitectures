window.onload = async function () {
    setTimeout(autologin, 1000);
};

async function autologin() {
    try {
		await connection.invoke("Login");
    } catch (err) {
        console.error(err);
    }
}

async function login () {
	var user = document.getElementById("login_user").value;
	var pass = document.getElementById("login_pass").value;
	try {
		await connection.invoke("LoginReq", user, pass);
    } catch (err) {
        console.error(err);
    }
};