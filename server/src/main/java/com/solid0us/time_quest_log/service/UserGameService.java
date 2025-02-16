package com.solid0us.time_quest_log.service;

import com.solid0us.time_quest_log.model.*;
import com.solid0us.time_quest_log.repositories.GameRepository;
import com.solid0us.time_quest_log.repositories.UserGameRepository;
import jakarta.transaction.Transactional;
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
    public ServiceResult<UserGames> upsertUserGame(UUID uuid, UserGames userGame) {
        List<ErrorDetail> errorDetails = new ArrayList<>();
        Games existingGame = gameRepository.findById(userGame.getGame().getId()).orElse(null);
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
}
