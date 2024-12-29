package com.solid0us.time_quest_log.service;

import com.solid0us.time_quest_log.model.UserGames;
import com.solid0us.time_quest_log.repositories.UserGameRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Service;

import java.util.List;
import java.util.UUID;

@Service
public class UserGameService {
    @Autowired
    private UserGameRepository userGameRepository;

    public UserGames saveUserGame(UserGames userGame) {
        return userGameRepository.save(userGame);

    }

    public List<UserGames> getUserGamesByUserId(UUID userId) {
        return userGameRepository.findByUser_Id(userId);
    }
}
