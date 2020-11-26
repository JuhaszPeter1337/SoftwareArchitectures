window.onload = async function () {
  setTimeout(getevents, 250);
};

async function getevents() {
  var login = sessionStorage.getItem("login");
  if(login){
    try {
        await connection.invoke("GetEvents");
      } catch (err) {
        console.error(err);
      }
  }
  else{
      window.location.href = "/index.html";
  }
}