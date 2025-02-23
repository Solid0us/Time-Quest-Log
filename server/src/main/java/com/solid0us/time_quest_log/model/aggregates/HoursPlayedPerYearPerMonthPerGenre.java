package com.solid0us.time_quest_log.model.aggregates;

public class HoursPlayedPerYearPerMonthPerGenre {
    private int id;
    private String genre;
    private String year;
    private String month;
    private double hoursPlayed;

    public HoursPlayedPerYearPerMonthPerGenre(int id, String genre, String year, String month, double hoursPlayed) {
        this.id = id;
        this.genre = genre;
        this.year = year;
        this.month = month;
        this.hoursPlayed = hoursPlayed;
    }

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
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

    public String getGenre() {
        return genre;
    }

    public void setGenre(String genre) {
        this.genre = genre;
    }

    public double getHoursPlayed() {
        return hoursPlayed;
    }

    public void setHoursPlayed(double hoursPlayed) {
        this.hoursPlayed = hoursPlayed;
    }
}
