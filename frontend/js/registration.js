function check() {
    if (document.getElementById('mypassword').value == document.getElementById('mypassword_again').value) {
      document.getElementById('message').style.color = 'green';
      document.getElementById('message').innerHTML = 'Password is matching';
      document.getElementById("next_button").disabled = false;
    } else {
      document.getElementById('message').style.color = 'red';
      document.getElementById('message').innerHTML = 'Password is not matching';
      document.getElementById("next_button").disabled = true;
    }
}

async function sendDatas() {
    var usernameValue = document.getElementById("uniqueusername").value;
    var passwordValue = document.getElementById("mypassword").value;

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

    user.setUsername(usernameValue);
    user.setPassword(passwordValue);

    user.setInterests(interests1, interests2, interests3, interests4, interests5, interests6);
    user.setLanguages(language1, language2, language3, language4, language5, language6);

    sessionStorage.setItem("user", user);

    try {
      await connection.invoke("RegisterReq", user);
    } catch (err) {
      console.error(err);
    }
}
