package com.solid0us.time_quest_log.model.aggregates;

public class HoursPlayedByGenre {
    private int id;
    private String name;
    private double hoursPlayed;

    public HoursPlayedByGenre(int id, String name, double hoursPlayed) {
        this.id = id;
        this.name = name;
        this.hoursPlayed = hoursPlayed;
    }

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public double getHoursPlayed() {
        return hoursPlayed;
    }

    public void setHoursPlayed(double hoursPlayed) {
        this.hoursPlayed = hoursPlayed;
    }
}
