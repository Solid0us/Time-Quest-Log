package com.solid0us.time_quest_log.service;

import com.solid0us.time_quest_log.model.GameSessions;
import com.solid0us.time_quest_log.repositories.GameSessionRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.List;
import java.util.UUID;

@Service
public class GameSessionService {
    @Autowired
    private GameSessionRepository gameSessionRepository;

    public GameSessions createGameSession(GameSessions gameSession) {
        return gameSessionRepository.save(gameSession);
    }

    public List<GameSessions> getGameSessions(String userId) {
        if (userId == null) {
            return gameSessionRepository.findAll();
        }
        return gameSessionRepository.findByUser_Id(UUID.fromString(userId));
    }

}
