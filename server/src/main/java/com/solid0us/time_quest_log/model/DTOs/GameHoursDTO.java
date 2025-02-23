package com.solid0us.time_quest_log.model.DTOs;

public class GameHoursDTO {
    private int gameId;
    private String gameTitle;
    private double hoursPlayed;

    public GameHoursDTO(int gameId, String gameTitle, double hoursPlayed) {
        this.gameId = gameId;
        this.gameTitle = gameTitle;
        this.hoursPlayed = hoursPlayed;
    }

    public int getGameId() {
        return gameId;
    }

    public void setGameId(int gameId) {
        this.gameId = gameId;
    }

    public String getGameTitle() {
        return gameTitle;
    }

    public void setGameTitle(String gameTitle) {
        this.gameTitle = gameTitle;
    }

    public double getHoursPlayed() {
        return hoursPlayed;
    }

    public void setHoursPlayed(double hoursPlayed) {
        this.hoursPlayed = hoursPlayed;
    }
}
