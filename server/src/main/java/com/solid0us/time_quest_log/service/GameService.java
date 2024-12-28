package com.solid0us.time_quest_log.service;

import com.solid0us.time_quest_log.model.IGDBGame;
import com.solid0us.time_quest_log.repositories.GameRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.List;

@Service
public class GameService {
    @Autowired
    private GameRepository gameRepository;

    @Autowired
    private IGDBService igdbService;

    public List<IGDBGame> getAllGames(String name) {
        try {
            return igdbService.searchGames(name);
        } catch (InterruptedException e) {
            throw new RuntimeException(e);
        }
    }
}
