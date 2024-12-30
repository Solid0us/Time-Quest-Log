package com.solid0us.time_quest_log.service;

import com.solid0us.time_quest_log.model.Games;
import com.solid0us.time_quest_log.model.UserGames;
import com.solid0us.time_quest_log.repositories.UserGameRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.dao.DataIntegrityViolationException;
import org.springframework.stereotype.Service;

import java.util.List;
import java.util.Optional;
import java.util.UUID;

@Service
public class UserGameService {
    @Autowired
    private UserGameRepository userGameRepository;

    @Autowired
    private GameService gameService;

    public UserGames saveUserGame(UserGames userGame) {
        Optional<Games> existingGame = gameService.getGameById(userGame.getGame().getId());
        if (!existingGame.isPresent()) {
            gameService.createGame(userGame.getGame());
        }
        try {
            return userGameRepository.save(userGame);
        } catch (DataIntegrityViolationException e) {
            System.out.println(e.getMessage());
            throw new RuntimeException("Game already exists in the user's library.", e);
        } catch (Exception e) {
            throw new RuntimeException("Error saving UserGame", e);
        }

    }

    public List<UserGames> getUserGamesByUserId(UUID userId) {
        return userGameRepository.findByUser_Id(userId);
    }
}
