package com.solid0us.time_quest_log.service;

import com.solid0us.time_quest_log.model.ErrorDetail;
import com.solid0us.time_quest_log.model.Games;
import com.solid0us.time_quest_log.model.ServiceResult;
import com.solid0us.time_quest_log.model.UserGames;
import com.solid0us.time_quest_log.repositories.UserGameRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.dao.DataIntegrityViolationException;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.List;
import java.util.Optional;
import java.util.UUID;

@Service
public class UserGameService {
    @Autowired
    private UserGameRepository userGameRepository;

    @Autowired
    private GameService gameService;

    public ServiceResult<UserGames> saveUserGame(UserGames userGame) {
        ServiceResult<Optional<Games>> gameExistsResult = gameService.getGameById(userGame.getGame().getId());
        List<ErrorDetail> errorDetails = new ArrayList<>();
        if (!gameExistsResult.isSuccess()) {
            gameService.createGame(userGame.getGame());
        }
        try {
            return ServiceResult.success(userGameRepository.save(userGame));
        } catch (DataIntegrityViolationException e) {
            System.out.println(e.getMessage());
            errorDetails.add(new ErrorDetail("gameId", "Game already exists in the user's library."));
        } catch (Exception e) {
            errorDetails.add(new ErrorDetail("input", "Unknown error occurred while saving game to user's library."));
        }
        return ServiceResult.failure(errorDetails);
    }

    public ServiceResult<List<UserGames>> getUserGamesByUserId(UUID userId) {
        return ServiceResult.success(userGameRepository.findByUser_Id(userId));
    }
}
