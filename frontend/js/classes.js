class Profile{
    constructor(){}

    getUsername(){
        return this.username;
    }

    setUsername(username){
        this.username = username;
    }

    getInterests(){
        return this.interests;
    }

    setInterests(first, second, third, fourth, fifth, sixth){
        this.interests = first | second * 2 | third * 4 | fourth * 8 | fifth * 16 | sixth * 32;
    }

    getLanguages(){
        return this.languages;
    }

    setLanguages(first, second, third, fourth, fifth, sixth){
        this.languages = first | second * 2 | third * 4 | fourth * 8 | fifth * 16 | sixth * 32;
    }

    getFavorites(){
        return this.favorites;
    }

    setFavorites(){
        this.favorites = [];
    }
}

class Event{
    constructor(){}

    getTitle(){
        return this.title;
    }

    setTitle(title){
        this.title = title;
    }

    getBegin(){
        return this.begin;
    }

    setBegin(begin){
        this.beginning = begin;
    }

    getEnd(){
        return this.end;
    }

    setEnd(end){
        this.ending = end;
    }

    getDescription(){
        return this.description;
    }

    setDescription(description){
        this.description = description;
    }

    getInterests(){
        return this.interests;
    }

    setInterests(first, second, third, fourth, fifth, sixth){
        this.interests = first | second * 2 | third * 4 | fourth * 8 | fifth * 16 | sixth * 32;
    }

    getLanguages(){
        return this.languages;
    }

    setLanguages(first, second, third, fourth, fifth, sixth){
        this.languages = first | second * 2 | third * 4 | fourth * 8 | fifth * 16 | sixth * 32;
    }
}

var interestsEnum = {
    WatchSports: 1,
    PlaySports: 2, 
    Cinema: 4, 
    Museum: 8, 
    Hiking: 16,
    Cooking: 32
}

var languagesEnum = {
    English: 1,
    German: 2, 
    French: 4, 
    Spanish: 8, 
    Russian: 16,
    Hungarian: 32
}