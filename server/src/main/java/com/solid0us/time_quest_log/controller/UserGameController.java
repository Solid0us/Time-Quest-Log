package com.solid0us.time_quest_log.controller;

import com.solid0us.time_quest_log.model.UserGames;
import com.solid0us.time_quest_log.service.UserGameService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.UUID;

@RestController
@RequestMapping("/api/v1/user-games")
public class UserGameController {
    @Autowired
    private UserGameService userGameService;

    @PostMapping({"/", ""})
    public ResponseEntity<?> saveUserGame(@RequestBody UserGames userGame) {
        try {
            return ResponseEntity.status(HttpStatus.CREATED).body(userGameService.saveUserGame(userGame));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.BAD_REQUEST).body(e.getMessage());
        }
    }

    @GetMapping({"/{userId}", "/{userId}/"})
    public ResponseEntity<?> getUserGamesByUserId(@PathVariable String userId) {
        return ResponseEntity.ok(userGameService.getUserGamesByUserId(UUID.fromString(userId)));
    }
}
