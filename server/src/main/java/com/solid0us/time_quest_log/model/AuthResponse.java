package com.solid0us.time_quest_log.model;

import java.util.Date;

public class AuthResponse {
    private final Date timestamp;
    private final String username;
    private final String token;
    private final String error;

    public AuthResponse(String token, String username) {
        this.token = token;
        this.timestamp = new Date();
        this.username = username;
        this.error = null;
    }

    public AuthResponse(String token, String username, String error){
        this.token = token;
        this.timestamp = new Date();
        this.username = username;
        this.error = error;
    }

    public Date getTimestamp() {
        return timestamp;
    }

    public String getUsername() {
        return username;
    }

    public String getToken() {
        return token;
    }

    public String getError() {
        return error;
    }
}
