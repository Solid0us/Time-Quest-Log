package com.solid0us.time_quest_log.service;

import com.solid0us.time_quest_log.model.*;
import com.solid0us.time_quest_log.model.DTOs.GameHoursDTO;
import com.solid0us.time_quest_log.model.DTOs.GameSessionAggregateStatsDTO;
import com.solid0us.time_quest_log.model.DTOs.GameStatsDTO;
import com.solid0us.time_quest_log.model.DTOs.UserGameWithHoursDTO;
import com.solid0us.time_quest_log.model.aggregates.HoursPlayedByGenre;
import com.solid0us.time_quest_log.model.aggregates.HoursPlayedPerYear;
import com.solid0us.time_quest_log.model.aggregates.HoursPlayedPerYearPerMonthPerGame;
import com.solid0us.time_quest_log.model.aggregates.HoursPlayedPerYearPerMonthPerGenre;
import com.solid0us.time_quest_log.repositories.GameRepository;
import com.solid0us.time_quest_log.repositories.GameSessionRepository;
import com.solid0us.time_quest_log.repositories.UserGameRepository;
import jakarta.transaction.Transactional;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.dao.DataIntegrityViolationException;
import org.springframework.stereotype.Service;

import java.util.*;

@Service
public class UserGameService {
    @Autowired
    private GameSessionRepository gameSessionRepository;

    @Autowired
    private UserGameRepository userGameRepository;

    @Autowired
    private GameRepository gameRepository;

    @Autowired
    private GameGenreService gameGenreService;

    @Autowired
    private IGDBService igdbService;

    @Autowired
    private GameService gameService;

    @Transactional(rollbackOn = Exception.class)
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

    @Transactional(rollbackOn = Exception.class)
    public ServiceResult<List<UserGames>> getUserGamesByUserId(UUID userId) {
        return ServiceResult.success(userGameRepository.findByUser_Id(userId));
    }

    @Transactional
    public ServiceResult<List<UserGameWithHoursDTO>> getUserGameWithHoursById(UUID userId) {
        return ServiceResult.success(userGameRepository.getUserGameWithHoursPlayed(userId));
    }

    @Transactional
    public ServiceResult<UserGames> upsertUserGame(UUID uuid, UserGames userGame) {
        List<ErrorDetail> errorDetails = new ArrayList<>();
        Games existingGame = gameRepository.findById(userGame.getGame().getId()).orElse(null);
        int hoursPlayed = 0;

        if (existingGame == null) {
            try {
                ServiceResult<IGDBGame> gameServiceResult = igdbService.searchGameById(userGame.getGame().getId());
                if (!gameServiceResult.isSuccess()){
                    errorDetails.add(new ErrorDetail("gameId", "Game not found in IGDB."));
                    return ServiceResult.failure(errorDetails);
                }
                IGDBGame game = gameServiceResult.getData();
                Games newGame = new Games();
                newGame.setId(game.getId());
                newGame.setName(game.getName());
                newGame.setCoverUrl("https:" + game.getCover().getUrl());
                gameService.createGame(newGame);
                List<GameGenres> genres = new ArrayList<>();

                for (IGDBGenre genre : game.getGenres()){
                    GameGenres gameGenre = new GameGenres();
                    Genres newGenre = new Genres();
                    Games gameToAdd = new Games();
                    gameToAdd.setId(newGame.getId());
                    newGenre.setId(genre.getId());
                    newGenre.setName(genre.getName());
                    gameGenre.setId(new GameGenreIdComposite(newGame.getId(), newGenre.getId()));
                    genres.add(gameGenre);
                }
                gameGenreService.createGameGenres(genres);
            } catch (InterruptedException e) {
                errorDetails.add(new ErrorDetail("input", "Unknown error occurred while updating user's library"));
                return ServiceResult.failure(errorDetails);
            }
        }
        UserGames gameToSave = userGameRepository.findById(uuid)
                .map(game -> {
                    game.setGame(userGame.getGame());
                    game.setExeName(userGame.getExeName());
                    game.setUser(userGame.getUser());
                    return game;
                })
                .orElse(userGame);
        gameToSave.setId(uuid);
        return ServiceResult.success(userGameRepository.save(gameToSave));
    }

    public ServiceResult<GameStatsDTO> getUserGameStats(UUID uuid) {
        Double hoursPlayed = gameSessionRepository.findTotalPlaytimeInHoursByUserId(uuid);
        int distinctGames = userGameRepository.findDistinctGamesByUserId(uuid);
        List<HoursPlayedByGenre> hoursPlayedByGenre = gameSessionRepository.findTotalPlaytimeByGenre(uuid);
        HashMap<String, Double> hoursPlayedByGenreMap = new HashMap<>();
        hoursPlayedByGenre.forEach(hoursPlayedByGenre1 -> {
            hoursPlayedByGenreMap.put(hoursPlayedByGenre1.getName(), hoursPlayedByGenre1.getHoursPlayed());
        });
        List<GameHoursDTO> hoursPlayedPerGame = gameSessionRepository.findHoursPlayedPerGame(uuid);
        List<HoursPlayedPerYear> hoursPlayedPerYear = gameSessionRepository.findHoursPlayedPerYear(uuid);
        HashMap<String, Double> hoursPlayedPerYearMap = new HashMap<>();
        hoursPlayedPerYear.forEach(entry -> {
            hoursPlayedPerYearMap.put(entry.getYear(), entry.getHoursPlayed());
        });

        List<HoursPlayedPerYearPerMonthPerGenre> hoursPlayedPerYearPerMonthPerGenre = gameSessionRepository.findHoursPlayedPerYearPerMonthPerGenre(uuid);
        List<HoursPlayedPerYearPerMonthPerGame> hoursPlayedPerYearPerMonthPerGame = gameSessionRepository.findHoursPlayedPerYearPerMonthPerGame(uuid);


        GameStatsDTO stats = new GameStatsDTO(
                uuid.toString(),
                hoursPlayed,
                distinctGames,
                hoursPlayedByGenreMap,
                hoursPlayedPerGame,
                hoursPlayedPerYearMap,
                hoursPlayedPerYearPerMonthPerGenre,
                hoursPlayedPerYearPerMonthPerGame
        );

        return ServiceResult.success(stats);
    }

    public ServiceResult<GameSessionAggregateStatsDTO> getUserGameSessionAggregateStats(UUID userId, int gameId){
        try {
            List<GameSessionAggregateStatsDTO> stats = gameSessionRepository.findUserGameAggregateSessionStats(userId, gameId);
            return ServiceResult.success(stats.get(0));
        } catch (Exception e){
            return ServiceResult.failure(List.of(new ErrorDetail("gameId", "Game not found in user's library.")));
        }
    }
}
