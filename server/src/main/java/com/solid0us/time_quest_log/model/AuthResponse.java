package com.solid0us.time_quest_log.model;

import java.util.Date;
import java.util.UUID;

public class AuthResponse {
    private final Date timestamp;
    private final UUID userId;
    private final String username;
    private final String token;
    private final String refreshToken;
    private final String error;

    public AuthResponse(String token,  String refreshToken, Users user) {
        this.token = token;
        this.timestamp = new Date();
        this.username = user.getUsername();
        this.userId = user.getId();
        this.error = null;
        this.refreshToken = refreshToken;
    }

    public AuthResponse(String username, String error){
        this.userId = null;
        this.token = null;
        this.timestamp = new Date();
        this.username = username;
        this.error = error;
        this.refreshToken = null;
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

    public String getRefreshToken() { return refreshToken; }

    public UUID getUserId() { return userId; }
}
