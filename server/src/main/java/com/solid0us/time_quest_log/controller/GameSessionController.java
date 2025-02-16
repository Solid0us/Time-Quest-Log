package com.solid0us.time_quest_log.controller;

import com.solid0us.time_quest_log.model.*;
import com.solid0us.time_quest_log.service.GameSessionService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.ArrayList;
import java.util.List;

@RestController
@RequestMapping("/api/v1/game-sessions")
public class GameSessionController {
    @Autowired
    private GameSessionService gameSessionService;

    @GetMapping({"/", ""})
    public ResponseEntity<ApiResponse<?>> getGameSessions(@RequestParam(required = false) String userId) {
        ServiceResult<List<GameSessionsDTO>> result = gameSessionService.getGameSessions(userId);
        if (result.isSuccess()){
            return ResponseEntity.status(HttpStatus.OK)
                    .body(ApiResponse.success("", result.getData()));
        } else {
            return ResponseEntity.status(HttpStatus.BAD_REQUEST)
                    .body(ApiResponse.failure("Unable to retrieve game sessions.", result.getErrors()));
        }
    }

    @PostMapping({"/", ""})
    public ResponseEntity<ApiResponse<?>> createGameSession(@RequestBody GameSessions gameSession) {
        ServiceResult<GameSessionsDTO> result = gameSessionService.createGameSession(gameSession);
        if (result.isSuccess()){
           return ResponseEntity.status(HttpStatus.CREATED).body(ApiResponse.success("", result.getData()));
        } else {
            return ResponseEntity.status(HttpStatus.BAD_REQUEST)
                    .body(ApiResponse.failure("Unable to add game to user's library.", result.getErrors()));
        }

    }

    @PutMapping({"/{id}", "/{id}/"})
    public ResponseEntity<ApiResponse<?>> updateGameSession(@PathVariable String id, @RequestBody GameSessions gameSession) {
        ServiceResult<GameSessionsDTO> result = gameSessionService.updateGameSession(id, gameSession);
        if (result.isSuccess()){
            return ResponseEntity.status(HttpStatus.OK).body(ApiResponse.success("", result.getData()));
        } else {
            return ResponseEntity.status(HttpStatus.BAD_REQUEST)
                    .body(ApiResponse.failure("Unable to update game session.", result.getErrors()));
        }
    }

    @PutMapping({"/sync/{userId}", "/sync/{userId}/"})
    public ResponseEntity<ApiResponse<?>> syncUserGames(@PathVariable String userId, @RequestBody GameSessions[] gameSessions){
        List<ErrorDetail> errorDetails = new ArrayList<>();
        List<GameSessionsDTO> successfullySyncedSessions = new ArrayList<>();
        for (GameSessions game : gameSessions){
            ServiceResult<GameSessionsDTO> result = gameSessionService.updateGameSession(game.getId().toString(), game);
            if(result.isSuccess()){
                successfullySyncedSessions.add(result.getData());
            } else {
                errorDetails.addAll(result.getErrors());
            }
        }
        if(errorDetails.isEmpty()){
            return ResponseEntity.status(HttpStatus.OK).body(ApiResponse.success("All user games have been synced", successfullySyncedSessions));
        }
        return ResponseEntity.status(HttpStatus.BAD_REQUEST)
                .body(ApiResponse.failure("One or more games could not be synced.",successfullySyncedSessions, errorDetails));
    }
}
