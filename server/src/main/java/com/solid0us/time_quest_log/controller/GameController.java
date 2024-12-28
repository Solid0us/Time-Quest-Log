package com.solid0us.time_quest_log.controller;

import com.solid0us.time_quest_log.model.IGDBGame;
import com.solid0us.time_quest_log.service.GameService;
import org.springframework.beans.factory.annotation.Autowired;
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
    public List<IGDBGame> getGames(
            @RequestParam(required = false) String name
    ) {
        return gameService.getAllGames(name);
    }
}
