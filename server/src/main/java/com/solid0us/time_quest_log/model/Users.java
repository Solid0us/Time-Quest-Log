package com.solid0us.time_quest_log.model;

import jakarta.persistence.*;
import org.hibernate.annotations.CreationTimestamp;

import java.sql.Timestamp;
import java.util.UUID;

@Entity
@Table (name = "users",
 uniqueConstraints = {@UniqueConstraint(columnNames = {"username"})})
public class Users {
    @Id
    @GeneratedValue(strategy = GenerationType.UUID)
    private UUID id;

    @Column(nullable = false, length = 20, unique = true)
    private String username;

    @Column(nullable = false,  length = 20, name="first_name")
    private String firstName;

    @Column(nullable = false, length = 20, name="last_name")
    private String lastName;

    @Column(nullable = false)
    private String password;

    @CreationTimestamp
    @Column(nullable = false)
    private Timestamp createdAt;

    public Users() {
    }

    public Users(UUID id, String username, String firstName, String lastName, String password) {
        this.id = id;
        this.username = username;
        this.firstName = firstName;
        this.lastName = lastName;
        this.password = password;
    }

    public UUID getId() {
        return id;
    }

    public void setId(UUID id) {
        this.id = id;
    }

    public String getUsername() {
        return username;
    }

    public void setUsername(String username) {
        this.username = username;
    }

    public String getFirstName() {
        return firstName;
    }

    public void setFirstName(String firstName) {
        this.firstName = firstName;
    }

    public String getLastName() {
        return lastName;
    }

    public void setLastName(String lastName) {
        this.lastName = lastName;
    }

    public String getPassword() {
        return password;
    }

    public void setPassword(String password) {
        this.password = password;
    }
}