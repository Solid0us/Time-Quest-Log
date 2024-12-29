package com.solid0us.time_quest_log.model;

import jakarta.persistence.*;

@Entity
@Table(name = "genres")
public class Genres {
    @Id
    private int id;

    @Column(name = "name", nullable = false, unique = true)
    private String name;


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

}
