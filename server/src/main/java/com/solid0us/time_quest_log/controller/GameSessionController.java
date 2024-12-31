package com.solid0us.time_quest_log.controller;

import com.solid0us.time_quest_log.model.ApiResponse;
import com.solid0us.time_quest_log.model.GameSessions;
import com.solid0us.time_quest_log.model.ServiceResult;
import com.solid0us.time_quest_log.service.GameSessionService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequestMapping("/api/v1/game-sessions")
public class GameSessionController {
    @Autowired
    private GameSessionService gameSessionService;

    @GetMapping({"/", ""})
    public ResponseEntity<ApiResponse<?>> getGameSessions(@RequestParam(required = false) String userId) {
        ServiceResult<List<GameSessions>> result = gameSessionService.getGameSessions(userId);
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
        ServiceResult<GameSessions> result =gameSessionService.createGameSession(gameSession);
        if (result.isSuccess()){
           return ResponseEntity.status(HttpStatus.CREATED).body(ApiResponse.success("", result.getData()));
        } else {
            return ResponseEntity.status(HttpStatus.BAD_REQUEST)
                    .body(ApiResponse.failure("Unable to add game to user's library.", result.getErrors()));
        }

    }
}
