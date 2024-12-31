package com.solid0us.time_quest_log.controller;

import com.solid0us.time_quest_log.model.ApiResponse;
import com.solid0us.time_quest_log.model.ErrorDetail;
import com.solid0us.time_quest_log.model.IGDBGame;
import com.solid0us.time_quest_log.model.ServiceResult;
import com.solid0us.time_quest_log.service.GameService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

import java.util.List;

@RestController
@RequestMapping("/api/v1/games")
public class GameController {

    @Autowired
    private GameService gameService;

    @GetMapping({"/", ""})
    public ResponseEntity<?> getGames(
            @RequestParam(required = false) String name
    ) {
        ServiceResult<List<IGDBGame>> result = gameService.getAllGames(name);
        if (result.isSuccess()) {
            return ResponseEntity.ok().body(ApiResponse.success("", result.getData()));
        } else {
            return ResponseEntity.ok().body(ApiResponse.failure("Could not get list of game data.", result.getErrors()));
        }
    }
}
