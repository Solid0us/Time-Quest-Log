package com.solid0us.time_quest_log.model.DTOs;


public class GameSessionAggregateStatsDTO {
    private double hoursPlayed;
    private double avgSessionTime;
    private double longestSessionTime;
    private String longestSessionDate;
    private String lastPlayed;
    private String firstTimePlayed;


    public GameSessionAggregateStatsDTO(double hoursPlayed, double avgSessionTime, double longestSessionTime, String longestSessionDate, String lastPlayed, String firstTimePlayed) {
        this.hoursPlayed = hoursPlayed;
        this.avgSessionTime = avgSessionTime;
        this.longestSessionTime = longestSessionTime;
        this.longestSessionDate = longestSessionDate;
        this.lastPlayed = lastPlayed;
        this.firstTimePlayed = firstTimePlayed;
    }

    public double getHoursPlayed() {
        return hoursPlayed;
    }

    public void setHoursPlayed(double hoursPlayed) {
        this.hoursPlayed = hoursPlayed;
    }

    public double getAvgSessionTime() {
        return avgSessionTime;
    }

    public void setAvgSessionTime(double avgSessionTime) {
        this.avgSessionTime = avgSessionTime;
    }

    public double getLongestSessionTime() {
        return longestSessionTime;
    }

    public void setLongestSessionTime(double longestSessionTime) {
        this.longestSessionTime = longestSessionTime;
    }

    public String getLastPlayed() {
        return lastPlayed;
    }

    public void setLastPlayed(String lastPlayed) {
        this.lastPlayed = lastPlayed;
    }

    public String getLongestSessionDate() {
        return longestSessionDate;
    }

    public void setLongestSessionDate(String longestSessionDate) {
        this.longestSessionDate = longestSessionDate;
    }

    public String getFirstTimePlayed() {
        return firstTimePlayed;
    }

    public void setFirstTimePlayed(String firstTimePlayed) {
        this.firstTimePlayed = firstTimePlayed;
    }
}
