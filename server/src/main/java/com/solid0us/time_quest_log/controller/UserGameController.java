package com.solid0us.time_quest_log.controller;

import com.solid0us.time_quest_log.model.*;
import com.solid0us.time_quest_log.model.DTOs.GameStatsDTO;
import com.solid0us.time_quest_log.service.UserGameService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.ArrayList;
import java.util.List;
import java.util.UUID;

@RestController
@RequestMapping("/api/v1/user-games")
public class UserGameController {
    @Autowired
    private UserGameService userGameService;

    @PostMapping({"/", ""})
    public ResponseEntity<ApiResponse<?>> saveUserGame(@RequestBody UserGames userGame) {
        ServiceResult<UserGames> result = userGameService.saveUserGame(userGame);
        if (result.isSuccess()){
            return ResponseEntity.status(HttpStatus.CREATED).body(ApiResponse.success("", result.getData()));
        } else {
            return ResponseEntity.status(HttpStatus.BAD_REQUEST)
                    .body(ApiResponse.failure("Unable to add game to user's library.", result.getErrors()));
        }
    }

    @GetMapping({"/{userId}", "/{userId}/"})
    public ResponseEntity<ApiResponse<?>> getUserGamesByUserId(@PathVariable String userId) {
        try {
            ServiceResult<List<UserGames>> result = userGameService.getUserGamesByUserId(UUID.fromString(userId));
            if (result.isSuccess()){
                for (UserGames game : result.getData()){
                    game.getUser().setPassword(null);
                }
                return ResponseEntity.status(HttpStatus.OK).body(ApiResponse.success("", result.getData()));
            } else {
                return ResponseEntity.status(HttpStatus.BAD_REQUEST)
                        .body(ApiResponse.failure("Unable to add game to user's library.", result.getErrors()));
            }
        } catch (IllegalArgumentException e) {
            return ResponseEntity.status(HttpStatus.BAD_REQUEST)
                    .body(ApiResponse.failure("Invalid UUID provided.", null));
        }
    }

    @GetMapping({"/{userId}/stats", "/{userId}/stats/" })
    public ResponseEntity<ApiResponse<?>> getUserGameStats(@PathVariable String userId) {
        try {
            ServiceResult<GameStatsDTO> result = userGameService.getUserGameStats(UUID.fromString(userId));
            if (result.isSuccess()){
                return ResponseEntity.status(HttpStatus.OK).body(ApiResponse.success("", result.getData()));
            } else {
                return ResponseEntity.status(HttpStatus.BAD_REQUEST)
                        .body(ApiResponse.failure("Unable to add game to user's library.", result.getErrors()));
            }
        } catch (IllegalArgumentException e) {
            return ResponseEntity.status(HttpStatus.BAD_REQUEST)
                    .body(ApiResponse.failure("Invalid UUID provided.", null));
        }
    }

    @PutMapping({"/{userGameId}", "/{userGameId}/"})
    public ResponseEntity<ApiResponse<?>> updateUserGame(@PathVariable String userGameId, @RequestBody UserGames userGame) {
        List<ErrorDetail> errors = new ArrayList<>();
        try {
            ServiceResult<UserGames> result = userGameService.upsertUserGame(UUID.fromString(userGameId), userGame);
            if (result.isSuccess()){
                return ResponseEntity.status(HttpStatus.OK).body(ApiResponse.success("", result.getData()));
            } else {
                return ResponseEntity.status(HttpStatus.BAD_REQUEST)
                        .body(ApiResponse.failure("Unable to add game to user's library.", result.getErrors()));
            }
        } catch (IllegalArgumentException e) {
            errors.add(new ErrorDetail("userGameId", "Invalid UUID provided."));
            return ResponseEntity.status(HttpStatus.BAD_REQUEST)
                    .body(ApiResponse.failure("Invalid UUID provided.", errors));
        } catch (Exception e) {
            errors.add(new ErrorDetail("request", e.getMessage()));
            return ResponseEntity.status(HttpStatus.BAD_REQUEST)
                    .body(ApiResponse.failure("An unknown server error occurred.", errors));
        }
    }

    @PutMapping({"/sync/{userId}", "/sync/{userId}/"})
    public ResponseEntity<ApiResponse<?>> syncUserGames(@PathVariable String userId, @RequestBody UserGames[] userGames) {
        List<ErrorDetail> errorDetails = new ArrayList<>();
        List<UserGames> successfullySyncedUserGames = new ArrayList<>();
        for (UserGames game : userGames){
            ServiceResult<UserGames> result = userGameService.upsertUserGame(game.getId(), game);
            if(result.isSuccess()){
                successfullySyncedUserGames.add(result.getData());
            } else {
                errorDetails.addAll(result.getErrors());
            }
        }
        if(errorDetails.isEmpty()){
            return ResponseEntity.status(HttpStatus.OK).body(ApiResponse.success("All user games have been synced", successfullySyncedUserGames));
        }
        return ResponseEntity.status(HttpStatus.BAD_REQUEST)
                .body(ApiResponse.failure("One or more games could not be synced.",successfullySyncedUserGames, errorDetails));
    }
}
