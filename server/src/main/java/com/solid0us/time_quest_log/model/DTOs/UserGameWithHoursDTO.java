package com.solid0us.time_quest_log.model.DTOs;

public class UserGameWithHoursDTO {
    private int gameId;
    private String gameName;
    private String coverUrl;
    private String exeName;
    private String genres;
    private Double hoursPlayed;

    public UserGameWithHoursDTO(int gameId, String gameName, String coverUrl, String exeName, String genres, Double hoursPlayed) {
        this.gameId = gameId;
        this.gameName = gameName;
        this.coverUrl = coverUrl;
        this.exeName = exeName;
        this.genres = genres;
        this.hoursPlayed = hoursPlayed;
    }

    public int getGameId() {
        return gameId;
    }

    public void setGameId(int gameId) {
        this.gameId = gameId;
    }

    public String getGameName() {
        return gameName;
    }

    public void setGameName(String gameName) {
        this.gameName = gameName;
    }

    public String getCoverUrl() {
        return coverUrl;
    }

    public void setCoverUrl(String coverUrl) {
        this.coverUrl = coverUrl;
    }

    public String getExeName() {
        return exeName;
    }

    public void setExeName(String exeName) {
        this.exeName = exeName;
    }

    public String getGenres() {
        return genres;
    }

    public void setGenres(String genres) {
        this.genres = genres;
    }

    public Double getHoursPlayed() {
        return hoursPlayed;
    }

    public void setHoursPlayed(Double hoursPlayed) {
        this.hoursPlayed = hoursPlayed;
    }
}
