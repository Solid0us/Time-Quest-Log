package com.solid0us.time_quest_log.model;

public class RefreshJwtResponse {
    private final String token;

    public RefreshJwtResponse(String token) {
        this.token = token;
    }

    public String getToken() {
        return token;
    }
}
