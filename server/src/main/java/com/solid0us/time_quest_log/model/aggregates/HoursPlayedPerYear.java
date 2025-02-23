package com.solid0us.time_quest_log.model.aggregates;

public class HoursPlayedPerYear {
    private String year;
    private double hoursPlayed;

    public HoursPlayedPerYear(String year, double hoursPlayed) {
        this.year = year;
        this.hoursPlayed = hoursPlayed;
    }

    public String getYear() {
        return year;
    }

    public void setYear(String year) {
        this.year = year;
    }

    public double getHoursPlayed() {
        return hoursPlayed;
    }

    public void setHoursPlayed(double hoursPlayed) {
        this.hoursPlayed = hoursPlayed;
    }
}
