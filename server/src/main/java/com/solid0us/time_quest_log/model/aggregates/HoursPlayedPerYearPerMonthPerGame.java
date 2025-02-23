package com.solid0us.time_quest_log.model.aggregates;

public class HoursPlayedPerYearPerMonthPerGame {
    private int gameId;
    private String gameTitle;
    private String year;
    private String month;
    private double hoursPlayed;

    public HoursPlayedPerYearPerMonthPerGame(int gameId, String gameTitle, String year, String month, double hoursPlayed) {
        this.gameId = gameId;
        this.gameTitle = gameTitle;
        this.year = year;
        this.month = month;
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

    public String getYear() {
        return year;
    }

    public void setYear(String year) {
        this.year = year;
    }

    public String getMonth() {
        return month;
    }

    public void setMonth(String month) {
        this.month = month;
    }

    public double getHoursPlayed() {
        return hoursPlayed;
    }

    public void setHoursPlayed(double hoursPlayed) {
        this.hoursPlayed = hoursPlayed;
    }
}
