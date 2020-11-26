function makePasswordEnable(){
    document.getElementById("passi-1").disabled = false;
    document.getElementById("passi-2").disabled = false;
    document.getElementById("passi-3").disabled = false;
}

var login = localStorage.getItem("login");
if(login){
    var u = localStorage.getItem("user");
    u = u ? JSON.parse(u) : undefined;
    if(u != undefined){
        document.getElementById("unamei").value= u.username;
        document.getElementById("interests1").checked= u.interests & 1;
        document.getElementById("interests2").checked= u.interests & 2;
        document.getElementById("interests3").checked= u.interests & 4;
        document.getElementById("interests4").checked= u.interests & 8;
        document.getElementById("interests5").checked= u.interests & 16;
        document.getElementById("interests6").checked= u.interests & 32;

        document.getElementById("language1").checked= u.languages & 1;
        document.getElementById("language2").checked= u.languages & 2;
        document.getElementById("language3").checked= u.languages & 4;
        document.getElementById("language4").checked= u.languages & 8;
        document.getElementById("language5").checked= u.languages & 16;
        document.getElementById("language6").checked= u.languages & 32;
    }    
}

function CheckPassword(inputtxt) 
{ 
  var passw= /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,}$/;
  if(inputtxt.match(passw)){ 
    return true;
  }
  else{
    return false;
  }
}

async function sendDatas(){

    var uname = document.getElementById("unamei").value;
    var pass1 = document.getElementById("passi-1").value;
    var pass2 = document.getElementById("passi-2").value;
    var pass3 = document.getElementById("passi-3").value;
    
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

    if(!pass1 && !pass2 && !pass3){
        var pass = null;
        var check = true;
    }
    else{
        var check = CheckPassword(pass1) && CheckPassword(pass2) && CheckPassword(pass3) && pass2 == pass3;
        var pass = {​​ ChangePassDTO: {​​
            currentpass: pass1,
            newpass: pass2
        }​​}​​;       
    }
   
    if(check){
        var user = new Profile();

        user.setInterests(interests1, interests2, interests3, interests4, interests5, interests6);
        user.setLanguages(language1, language2, language3, language4, language5, language6);

        localStorage.setItem("user", JSON.stringify(user));

        try {
            await connection.invoke("EditProfile", user, pass);
        } catch (err) {
            console.error(err);
        }
    }
}

