var user = document.getElementById("login_user");
var pass = document.getElementById("login_pass");

async function login () {
    try {
    await connection.invoke("LoginReq", user, pass);
  } catch (err) {
    console.error(err);
    }
};
