function makePasswordEnable(){
    document.getElementById("passi").disabled = false;
}

var login = sessionStorage.getItem("login");
if(login){
    var u = sessionStorage.getItem("user");
    u = u ? JSON.parse(u) : undefined;
    if(u != undefined){
        document.getElementById("unamei").value= u.username;
        document.getElementById("interests1").checked= u.interests[0];
        document.getElementById("interests2").checked= u.interests[1];
        document.getElementById("interests3").checked= u.interests[2];
        document.getElementById("interests4").checked= u.interests[3];
        document.getElementById("interests5").checked= u.interests[4];
        document.getElementById("interests6").checked= u.interests[5];

        document.getElementById("language1").checked= u.languages[0];
        document.getElementById("language2").checked= u.languages[1];
        document.getElementById("language3").checked= u.languages[2];
        document.getElementById("language4").checked= u.languages[3];
        document.getElementById("language5").checked= u.languages[4];
        document.getElementById("language6").checked= u.languages[5];
    }    
}

async function sendDatas(){
    var pass = document.getElementById("passi").value;
    
    if(pass == "***************")
        pass=null;
  
    var interests1 = document.getElementById("interests1").checked;
    var interests2 = document.getElementById("interests2").checked;
    var interests3 = document.getElementById("interests3").checked;
    var interests4 = document.getElementById("interests4").checked;
    var interests5 = document.getElementById("interests5").checked;
    var interests6 = document.getElementById("interests6").checked;
    
    var language1 = document.getElementById("language1").checked;
    var language2 = document.getElementById("language2").checked;
    var language3 = document.getElementById("language3").checked;
    var language4 = document.getElementById("language4").checked;
    var language5 = document.getElementById("language5").checked;
    var language6 = document.getElementById("language6").checked;

    var user = new Profile();

    user.setPassword(passwordValue);

    user.setInterests(interests1, interests2, interests3, interests4, interests5, interests6);
    user.setLanguages(language1, language2, language3, language4, language5, language6);

    sessionStorage.setItem("user", user);

    try {
      await connection.invoke("EditProfile", user);
    } catch (err) {
      console.error(err);
    }
}

