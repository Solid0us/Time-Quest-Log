package com.solid0us.time_quest_log.service;

import com.fasterxml.jackson.databind.ObjectMapper;
import com.solid0us.time_quest_log.model.IGDBGame;
import com.solid0us.time_quest_log.model.IGDBTokenResponse;
import com.solid0us.time_quest_log.model.exceptions.TwitchUnauthorizedException;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.stereotype.Service;

import java.io.IOException;
import java.net.URI;
import java.net.http.HttpClient;
import java.net.http.HttpRequest;
import java.net.http.HttpResponse;
import java.util.Collections;
import java.util.List;

@Service
public class IGDBService {
    @Value("${IGDB_CLIENT_ID}")
    private String clientId;

    @Value("${IGDB_SECRET_KEY}")
    private String clientSecretKey;

    private String accessToken;

    private String apiBaseUrl = "https://api.igdb.com/v4/";

    private final int MAX_RETRIES = 3;

    public IGDBService() {
        clientId = System.getenv("IGDB_CLIENT_ID");
        clientSecretKey = System.getenv("IGDB_SECRET_KEY");
        try {
            login();
        } catch (IOException | InterruptedException e) {
            throw new RuntimeException(e);
        }
    }

    public void login() throws IOException, InterruptedException {
        String loginUrl = String.format(
                "https://id.twitch.tv/oauth2/token?client_id=%s&client_secret=%s&grant_type=client_credentials",
                clientId,
                clientSecretKey);

        HttpResponse<String> response;
        try (HttpClient client = HttpClient.newHttpClient()) {
            HttpRequest request = HttpRequest.newBuilder()
                    .uri(URI.create(loginUrl))
                    .header("Content-Type", "application/json")
                    .POST(HttpRequest.BodyPublishers.ofString(""))
                    .build();

            response = client.send(request, HttpResponse.BodyHandlers.ofString());
        }
        ObjectMapper objectMapper = new ObjectMapper();
        IGDBTokenResponse tokenResponse = objectMapper.readValue(response.body(), IGDBTokenResponse.class);
        switch (response.statusCode()) {
            case 200:
                accessToken = tokenResponse.getAccess_token();
                System.out.println("Successfully logged into Twitch");
                break;
            case 400:
                System.out.println(tokenResponse.getMessage());
                break;
            default:
                System.out.println(tokenResponse.getMessage());
                System.out.println("Something went wrong.");
                break;
        }
    }

    public List<IGDBGame> searchGames(String searchString) throws IOException, InterruptedException, TwitchUnauthorizedException {
        String gamesUrl = apiBaseUrl + "games";
        String fields = "fields name, first_release_date, genres.*, cover.*;";
        String search = String.format("search \"%s\";", searchString);
        String limit = "limit 500;";
        String requestBody = fields + limit;
        if (!search.isBlank()) {
            requestBody += search;
        }
        int retries = 0;

        HttpResponse<String> response;
        do {
            try (HttpClient client = HttpClient.newHttpClient()) {
                HttpRequest request = HttpRequest.newBuilder()
                        .uri(URI.create(gamesUrl))
                        .header("Client-ID", clientId)
                        .header("Authorization", "Bearer " + accessToken)
                        .header("Content-Type", "text/plain")
                        .POST(HttpRequest.BodyPublishers.ofString(requestBody))
                        .build();

                response = client.send(request, HttpResponse.BodyHandlers.ofString());
                if (response.statusCode() == 401) {
                    login();
                    retries++;
                }
            }
        } while(retries < MAX_RETRIES && response.statusCode() == 401);

        if (retries == MAX_RETRIES) {
            throw new TwitchUnauthorizedException("Could not authorize with Twitch.");
        }
        if (response.statusCode() != 200) {
            return Collections.emptyList();
        }

        ObjectMapper objectMapper = new ObjectMapper();
        List<IGDBGame> games = objectMapper.readValue
                (response.body(),
                objectMapper
                .getTypeFactory()
                .constructCollectionType(List.class, IGDBGame.class
                ));
        return games;
    }
}
