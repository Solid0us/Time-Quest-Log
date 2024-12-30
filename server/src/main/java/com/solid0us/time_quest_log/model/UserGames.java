package com.solid0us.time_quest_log.model;

import jakarta.persistence.*;

import java.util.UUID;

@Entity
@Table(name = "user_games", uniqueConstraints = @UniqueConstraint(columnNames = {"user_id", "game_id"}))
public class UserGames {

    @Id
    private UUID id;

    @ManyToOne
    @JoinColumn(name = "user_id", referencedColumnName = "id", nullable = false)
    private Users user;

    @ManyToOne
    @JoinColumn(name = "game_id", referencedColumnName = "id", nullable = false)
    private Games game;

    @Column(name = "exe_name")
    private String exeName;

    public UserGames() {
        this.id = UUID.randomUUID();
    }

    public UUID getId() {
        return id;
    }

    public void setId(UUID id) {
        this.id = id;
    }

    public Users getUser() {
        return user;
    }

    public void setUser(Users user) {
        this.user = user;
    }

    public Games getGame() {
        return game;
    }

    public void setGame(Games game) {
        this.game = game;
    }

    public String getExeName() {
        return exeName;
    }

    public void setExeName(String exeName) {
        this.exeName = exeName;
    }
}
