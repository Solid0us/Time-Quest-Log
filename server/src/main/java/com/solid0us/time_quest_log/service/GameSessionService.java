package com.solid0us.time_quest_log.service;

import com.solid0us.time_quest_log.model.*;
import com.solid0us.time_quest_log.model.DTOs.GameSessionsDTO;
import com.solid0us.time_quest_log.repositories.GameRepository;
import com.solid0us.time_quest_log.repositories.GameSessionRepository;
import jakarta.transaction.Transactional;
import org.hibernate.exception.ConstraintViolationException;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.dao.DataIntegrityViolationException;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.List;
import java.util.UUID;

@Service
public class GameSessionService {
    @Autowired
    private GameSessionRepository gameSessionRepository;

    @Autowired
    private GameRepository  gameRepository;

    @Autowired
    private IGDBService igdbService;

    @Autowired
    private GameGenreService gameGenreService;

    @Transactional(rollbackOn = Exception.class)
    public ServiceResult<GameSessionsDTO> createGameSession(GameSessions gameSession) {
        List<ErrorDetail> errorDetails = new ArrayList<>();
        try {
            return ServiceResult.success(new GameSessionsDTO(gameSessionRepository.save(gameSession)));
        }  catch (DataIntegrityViolationException e) {
            errorDetails.add(new ErrorDetail("input", "There was an ID conflict when creating a game session."));
        } catch (ConstraintViolationException e){
            errorDetails.add(new ErrorDetail("input", "One or more entities do not exist."));
        }
        catch (Exception e) {
            errorDetails.add(new ErrorDetail("input", "Unknown error occurred while saving game session."));
        }
        return ServiceResult.failure(errorDetails);
    }

    @Transactional(rollbackOn = Exception.class)
    public ServiceResult<List<GameSessionsDTO>> getGameSessions() {
        List<ErrorDetail> errorDetails = new ArrayList<>();
        try {
            List<GameSessions> gameSessions = gameSessionRepository.findAll();
            List<GameSessionsDTO> gameSessionsDTO = new ArrayList<>();
            for (GameSessions gameSession : gameSessions) {
                gameSessionsDTO.add(new GameSessionsDTO(gameSession));
            }
            return ServiceResult.success(gameSessionsDTO);
        } catch (IllegalArgumentException e){
            errorDetails.add(new ErrorDetail("input", "Invalid UUID format."));
        } catch (Exception e) {
            errorDetails.add(new ErrorDetail("input", "Unknown error occurred while retrieving game sessions."));
        }
        return ServiceResult.failure(errorDetails);
    }

    @Transactional(rollbackOn = Exception.class)
    public ServiceResult<List<GameSessionsDTO>> getGameSessionByUser(String userId) {
        List<ErrorDetail> errorDetails = new ArrayList<>();
        try {
            List<GameSessions> gameSessions = gameSessionRepository.findByUser_Id(UUID.fromString(userId));
            List<GameSessionsDTO> gameSessionsDTO = new ArrayList<>();
            for (GameSessions gameSession : gameSessions) {
                gameSessionsDTO.add(new GameSessionsDTO(gameSession));
            }
            return ServiceResult.success(gameSessionsDTO);
        } catch (IllegalArgumentException e){
            errorDetails.add(new ErrorDetail("input", "Invalid UUID format."));
        } catch (Exception e) {
            errorDetails.add(new ErrorDetail("input", "Unknown error occurred while retrieving game sessions."));
        }
        return ServiceResult.failure(errorDetails);
    }

    @Transactional(rollbackOn = Exception.class)
    public ServiceResult<GameSessionsDTO> updateGameSession(String id, GameSessions gameSession) {
        List<ErrorDetail> errorDetails = new ArrayList<>();
        try {
            GameSessions existingGameSession = gameSessionRepository.findById(UUID.fromString(id)).orElse(null);
            if (existingGameSession == null) {
                Games existingGame = gameRepository.findById(gameSession.getGame().getId()).orElse(null);
                if (existingGame == null) {
                    ServiceResult<IGDBGame> result = null;
                    try {
                        result = igdbService.searchGameById(gameSession.getGame().getId());
                        IGDBGame gameToCreate = result.getData();
                        if (result.isSuccess()) {
                            Games newGame = new Games();
                            newGame.setId(gameToCreate.getId());
                            newGame.setName(gameToCreate.getName());
                            newGame.setCoverUrl(gameToCreate.getCover().getUrl());
                            gameRepository.save(newGame);
                            List<GameGenres> genres = new ArrayList<>();
                            for (IGDBGenre genre : gameToCreate.getGenres()){
                                GameGenres gameGenre = new GameGenres();
                                Genres newGenre = new Genres();
                                Games gameToAdd = new Games();
                                gameToAdd.setId(newGame.getId());
                                newGenre.setId(genre.getId());
                                newGenre.setName(genre.getName());
                                gameGenre.setId(new GameGenreIdComposite(newGame.getId(), newGenre.getId()));
                                genres.add(gameGenre);
                            }
                            gameSession.setGame(newGame);
                            gameGenreService.createGameGenres(genres);
                        } else {
                            errorDetails.add(new ErrorDetail("input", "Game not found."));
                        }
                    } catch (InterruptedException e) {
                        throw new RuntimeException(e);
                    }
                }
                try {
                    gameSession.setId(UUID.fromString(id));
                    GameSessions createdGameSession = gameSessionRepository.save(gameSession);
                    return ServiceResult.success(new GameSessionsDTO(createdGameSession));
                } catch (Exception e) {
                    errorDetails.add(new ErrorDetail("input", "Unknown error occurred while updating game session."));
                }
            }
            else {
                try {
                    GameSessions gameSessionToSave = gameSessionRepository.findById(UUID.fromString(id))
                            .map(session -> {
                                session.setGame(gameSession.getGame());
                                session.setStartTime(gameSession.getStartTime());
                                session.setEndTime(gameSession.getEndTime());
                                session.setUser(gameSession.getUser());
                                return session;
                            })
                            .orElse(gameSession);
                    return ServiceResult.success(new GameSessionsDTO(gameSessionRepository.save(gameSessionToSave)));
                } catch (IllegalArgumentException e){
                    errorDetails.add(new ErrorDetail("input", "Invalid UUID format."));
                }
            }
        } catch (IllegalArgumentException e){
            errorDetails.add(new ErrorDetail("input", "Invalid UUID format."));
        }
        return ServiceResult.failure(errorDetails);
    }
}
