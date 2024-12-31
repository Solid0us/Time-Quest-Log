package com.solid0us.time_quest_log.model;

import jakarta.persistence.*;
import jakarta.validation.constraints.Email;
import org.hibernate.annotations.CreationTimestamp;

import java.sql.Timestamp;
import java.util.UUID;

@Entity
@Table (name = "users",
 uniqueConstraints = {@UniqueConstraint(columnNames = {"username"})})
public class Users {
    @Id
    private UUID id;

    @Column(nullable = false, unique = true, name="email")
    @Email(message = "Invalid email")
    private String email;

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
        this.id = UUID.randomUUID();
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

    public @Email(message = "Invalid email") String getEmail() {
        return email;
    }

    public void setEmail(@Email(message = "Invalid email") String email) {
        this.email = email.toLowerCase();
    }

    public Timestamp getCreatedAt() {
        return createdAt;
    }

    public void setCreatedAt(Timestamp createdAt) {
        this.createdAt = createdAt;
    }
}