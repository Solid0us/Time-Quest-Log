package com.solid0us.time_quest_log.model;

public class IGDBGame {
    private int id;
    private IGDBCover cover;
    private int first_release_date;
    private IGDBGenre[] genres;
    private String name;

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public IGDBCover getCover() {
        return cover;
    }

    public void setCover(IGDBCover cover) {
        this.cover = cover;
    }

    public int getFirst_release_date() {
        return first_release_date;
    }

    public void setFirst_release_date(int first_release_date) {
        this.first_release_date = first_release_date;
    }

    public IGDBGenre[] getGenres() {
        return genres;
    }

    public void setGenres(IGDBGenre[] genres) {
        this.genres = genres;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }
}
