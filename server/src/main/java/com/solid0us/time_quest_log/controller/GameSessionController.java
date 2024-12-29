package com.solid0us.time_quest_log.controller;

import com.solid0us.time_quest_log.model.GameSessions;
import com.solid0us.time_quest_log.service.GameSessionService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequestMapping("/api/v1/game-sessions")
public class GameSessionController {
    @Autowired
    private GameSessionService gameSessionService;

    @GetMapping({"/", ""})
    public List<GameSessions> getGameSessions(@RequestParam(required = false) String userId) {
        return gameSessionService.getGameSessions(userId);
    }

    @PostMapping({"/", ""})
    public GameSessions createGameSession(@RequestBody GameSessions gameSession) {
        return gameSessionService.createGameSession(gameSession);
    }
}
