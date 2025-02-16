package com.solid0us.time_quest_log.service;

import com.solid0us.time_quest_log.model.ErrorDetail;
import com.solid0us.time_quest_log.model.Games;
import com.solid0us.time_quest_log.model.IGDBGame;
import com.solid0us.time_quest_log.model.ServiceResult;
import com.solid0us.time_quest_log.repositories.GameRepository;
import jakarta.transaction.Transactional;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.dao.DataIntegrityViolationException;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.List;
import java.util.Optional;

@Service
public class GameService {
    @Autowired
    private GameRepository gameRepository;

    @Autowired
    private IGDBService igdbService;

    public ServiceResult<List<IGDBGame>> getAllGames(String name) {
        try {
            return igdbService.searchGames(name);
        } catch (InterruptedException e) {
            throw new RuntimeException(e);
        }
    }

    @Transactional
    public ServiceResult<Optional<Games>> getGameById(int id){
        Optional<Games> game = Optional.ofNullable(gameRepository.findById(id).orElse(null));
        List<ErrorDetail> errorDetails = new ArrayList<>();

        if (game.isPresent()) {
            return ServiceResult.success(game);
        } else {
            ErrorDetail error = new ErrorDetail("gameId", "Game not found.");
            errorDetails.add(error);
        }
        return ServiceResult.failure(errorDetails);
    }

    @Transactional
    public ServiceResult<Games> createGame(Games game) {
        List<ErrorDetail> errorDetails = new ArrayList<>();
        try {
            return ServiceResult.success(gameRepository.save(game));
        } catch (DataIntegrityViolationException e){
            ErrorDetail error = new ErrorDetail("input", "Game already exists in the database.");
            errorDetails.add(error);
        } catch (Exception e){
            ErrorDetail error = new ErrorDetail("input", "Unknown error occurred while saving game to user's library.");
            errorDetails.add(error);
        }
        return ServiceResult.failure(errorDetails);
    }
}
