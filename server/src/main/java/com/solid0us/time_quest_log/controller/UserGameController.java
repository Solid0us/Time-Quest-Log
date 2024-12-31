package com.solid0us.time_quest_log.controller;

import com.solid0us.time_quest_log.model.ApiResponse;
import com.solid0us.time_quest_log.model.ServiceResult;
import com.solid0us.time_quest_log.model.UserGames;
import com.solid0us.time_quest_log.service.UserGameService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

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
        ServiceResult<List<UserGames>> result = userGameService.getUserGamesByUserId(UUID.fromString(userId));
        if (result.isSuccess()){
            return ResponseEntity.status(HttpStatus.CREATED).body(ApiResponse.success("", result.getData()));
        } else {
            return ResponseEntity.status(HttpStatus.BAD_REQUEST)
                    .body(ApiResponse.failure("Unable to add game to user's library.", result.getErrors()));
        }
    }
}
