class Profile{
    constructor(){}

    getUsername(){
        return this.username;
    }

    setUsername(username){
        this.username = username;
    }

    getPassword(){
        return this.password;
    }

    setPassword(password){
        this.password = password;
    }

    getInterests(){
        return this.interests;
    }

    setInterests(first, second, third, fourth, fifth, sixth){
        this.interests = [];
        this.interests[0] = first;
        this.interests[1] = second;
        this.interests[2] = third;
        this.interests[3] = fourth;
        this.interests[4] = fifth;
        this.interests[5] = sixth;
    }

    getLanguages(){
        return this.languages;
    }

    setLanguages(first, second, third, fourth, fifth, sixth){
        this.languages = [];
        this.languages[0] = first;
        this.languages[1] = second;
        this.languages[2] = third;
        this.languages[3] = fourth;
        this.languages[4] = fifth;
        this.languages[5] = sixth;
    }

    getFavorites(){
        return this.favorites;
    }

    setFavorites(){
        this.favorites = [];
    }
}