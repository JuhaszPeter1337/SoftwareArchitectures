window.onload = async function () {
  setTimeout(getevents, 250);
};

async function getevents() {
  var login = sessionStorage.getItem("login");
  if(login){
    var u = sessionStorage.getItem("user");
    u = u ? JSON.parse(u) : undefined;
    var uname = u.username;
      try {
          await connection.invoke("GetEvents", uname);
        } catch (err) {
          console.error(err);
        }
  }
  else{
      window.location.href = "/index.html";
  }
}