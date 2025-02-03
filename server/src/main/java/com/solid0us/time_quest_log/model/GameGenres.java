package com.solid0us.time_quest_log.model;

import jakarta.persistence.*;

@Entity
@Table(name = "game_genre")
public class GameGenres {
    @EmbeddedId
    private GameGenreIdComposite id;

    public GameGenres() {
    }

    public GameGenreIdComposite getId() {
        return id;
    }

    public void setId(GameGenreIdComposite id) {
        this.id = id;
    }

}
