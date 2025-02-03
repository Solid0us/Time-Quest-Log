package com.solid0us.time_quest_log.model;

import jakarta.persistence.Column;
import jakarta.persistence.Embeddable;

import java.util.Objects;

@Embeddable
public class GameGenreIdComposite {
    @Column(name = "game_id", columnDefinition = "UUID")
    private int gameId;

    @Column(name = "genre_id")
    private Integer genreId;

    // Default constructor
    public GameGenreIdComposite() {}

    // Parameterized constructor
    public GameGenreIdComposite(int gameId, Integer genreId) {
        this.gameId = gameId;
        this.genreId = genreId;
    }

    // Getters and setters
    public int getGameId() {
        return gameId;
    }

    public void setGameId(int gameId) {
        this.gameId = gameId;
    }

    public Integer getGenreId() {
        return genreId;
    }

    public void setGenreId(Integer genreId) {
        this.genreId = genreId;
    }

    // Override equals and hashCode
    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;
        GameGenreIdComposite that = (GameGenreIdComposite) o;
        return Objects.equals(gameId, that.gameId) &&
                Objects.equals(genreId, that.genreId);
    }

    @Override
    public int hashCode() {
        return Objects.hash(gameId, genreId);
    }
}
