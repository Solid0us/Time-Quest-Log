package com.solid0us.time_quest_log.service;

import com.fasterxml.jackson.databind.ObjectMapper;
import com.solid0us.time_quest_log.model.IGDBGame;
import com.solid0us.time_quest_log.model.IGDBGenre;
import com.solid0us.time_quest_log.model.IGDBTokenResponse;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.stereotype.Service;

import java.io.IOException;
import java.net.URI;
import java.net.http.HttpClient;
import java.net.http.HttpRequest;
import java.net.http.HttpResponse;
import java.util.Collections;
import java.util.List;
import java.util.concurrent.BlockingQueue;
import java.util.concurrent.CompletableFuture;
import java.util.concurrent.LinkedBlockingQueue;

@Service
public class IGDBService {
    @Value("${IGDB_CLIENT_ID}")
    private final String clientId;

    @Value("${IGDB_SECRET_KEY}")
    private final String clientSecretKey;

    private String accessToken;

    private final String apiBaseUrl = "https://api.igdb.com/v4/";
    private final BlockingQueue<Runnable> requestQueue = new LinkedBlockingQueue<>();
    private final Thread workerThread;

    private final int MAX_RETRIES = 3;
    // To accommodate for IGDB's 4 request per second rate limit
    private final int THREAD_BLOCK_MILLISECONDS = 300;

    public IGDBService() {
        clientId = System.getenv("IGDB_CLIENT_ID");
        clientSecretKey = System.getenv("IGDB_SECRET_KEY");

        workerThread = new Thread(() -> {
            while (true) {
                try {
                    Runnable task = requestQueue.take();
                    task.run();
                    Thread.sleep(THREAD_BLOCK_MILLISECONDS);
                } catch (InterruptedException e) {
                    Thread.currentThread().interrupt();
                }
            }
        });
        workerThread.start();

        try {
            login();
        } catch (IOException | InterruptedException e) {
            throw new RuntimeException(e);
        }
    }

    public void addRequest(Runnable request) {
        requestQueue.offer(request);
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

    public List<IGDBGame> searchGames(String searchString) throws InterruptedException {
        CompletableFuture<List<IGDBGame>> resultFuture = new CompletableFuture<>();

        addRequest(() -> {
            try {
                resultFuture.complete(executeSearchGames(searchString));
            } catch (Exception e) {
                resultFuture.completeExceptionally(e);
            }
        });

        return resultFuture.join();
    }

    public List<IGDBGenre> getGenres() throws InterruptedException {
        CompletableFuture<List<IGDBGenre>> resultFuture = new CompletableFuture<>();

        addRequest(() -> {
            try {
                resultFuture.complete(executeGetGenres());
            } catch (Exception e) {
                resultFuture.completeExceptionally(e);
            }
        });

        return resultFuture.join();
    }

    private List<IGDBGame> executeSearchGames(String searchString) throws IOException, InterruptedException{
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
                    Thread.sleep(THREAD_BLOCK_MILLISECONDS);
                }
            }
        } while(retries < MAX_RETRIES && response.statusCode() == 401);

        if (retries == MAX_RETRIES) {
            throw new IOException("Could not authorize with Twitch.");
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

    private List<IGDBGenre> executeGetGenres() throws IOException, InterruptedException {
        String genresUrl = apiBaseUrl + "genres";
        String fields = "fields *;";
        String limit = "limit 500;";
        String requestBody = fields + limit;
        int retries = 0;

        HttpResponse<String> response;
        do {
            try (HttpClient client = HttpClient.newHttpClient()) {
                HttpRequest request = HttpRequest.newBuilder()
                        .uri(URI.create(genresUrl))
                        .header("Client-ID", clientId)
                        .header("Authorization", "Bearer " + accessToken)
                        .header("Content-Type", "text/plain")
                        .POST(HttpRequest.BodyPublishers.ofString(requestBody))
                        .build();

                response = client.send(request, HttpResponse.BodyHandlers.ofString());
            }
            if (response.statusCode() == 401) {
                login();
                retries++;
                Thread.sleep(THREAD_BLOCK_MILLISECONDS);
            }
        } while (retries < MAX_RETRIES && response.statusCode() == 401);

        if (retries == MAX_RETRIES) {
            throw new IOException("Could not authorize with Twitch.");
        }

        if (response.statusCode() != 200) {
            return Collections.emptyList();
        }

        ObjectMapper objectMapper = new ObjectMapper();
        List<IGDBGenre> genres = objectMapper.readValue
                (response.body(),
                        objectMapper
                                .getTypeFactory()
                                .constructCollectionType(List.class, IGDBGenre.class
                                ));
        return genres;
    }

}
