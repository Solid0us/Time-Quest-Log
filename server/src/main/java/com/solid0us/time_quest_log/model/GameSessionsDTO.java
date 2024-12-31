package com.solid0us.time_quest_log.model;

import java.sql.Timestamp;
import java.util.UUID;

public class GameSessionsDTO {
    private UUID id;
    private UsersDTO user;
    private Games game;
    private Timestamp startTime;
    private Timestamp endTime;

    public GameSessionsDTO(GameSessions gameSession) {
        this.id = gameSession.getId();
        this.user = new UsersDTO(gameSession.getUser());
        this.game = gameSession.getGame();
        this.startTime = gameSession.getStartTime();
        this.endTime = gameSession.getEndTime();
    }

    public UUID getId() {
        return id;
    }

    public void setId(UUID id) {
        this.id = id;
    }

    public UsersDTO getUser() {
        return user;
    }

    public void setUser(UsersDTO user) {
        this.user = user;
    }

    public Games getGame() {
        return game;
    }

    public void setGame(Games game) {
        this.game = game;
    }

    public Timestamp getStartTime() {
        return startTime;
    }

    public void setStartTime(Timestamp startTime) {
        this.startTime = startTime;
    }

    public Timestamp getEndTime() {
        return endTime;
    }

    public void setEndTime(Timestamp endTime) {
        this.endTime = endTime;
    }
}
