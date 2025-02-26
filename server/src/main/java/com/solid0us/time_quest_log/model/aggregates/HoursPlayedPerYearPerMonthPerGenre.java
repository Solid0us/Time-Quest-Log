package com.solid0us.time_quest_log.model.aggregates;

public class HoursPlayedPerYearPerMonthPerGenre {
    private int genreId;
    private String genreName;
    private String year;
    private String month;
    private double hoursPlayed;

    public HoursPlayedPerYearPerMonthPerGenre(int id, String genre, String year, String month, double hoursPlayed) {
        this.genreId = id;
        this.genreName = genre;
        this.year = year;
        this.month = month;
        this.hoursPlayed = hoursPlayed;
    }

    public int getGenreId() {
        return genreId;
    }

    public void setGenreId(int genreId) {
        this.genreId = genreId;
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

    public String getGenreName() {
        return genreName;
    }

    public void setGenreName(String genreName) {
        this.genreName = genreName;
    }

    public double getHoursPlayed() {
        return hoursPlayed;
    }

    public void setHoursPlayed(double hoursPlayed) {
        this.hoursPlayed = hoursPlayed;
    }
}
