window.onload = async function () {
    setTimeout(getfavorites, 250);
  };
  
  async function getfavorites() {
    var login = sessionStorage.getItem("login");
    if(login){
      var u = sessionStorage.getItem("user");
      u = u ? JSON.parse(u) : undefined;
      var uname = u.username;
        try {
            await connection.invoke("GetFavorites", uname);
          } catch (err) {
            console.error(err);
          }
    }
    else{
        window.location.href = "/main.html";
    }
  }