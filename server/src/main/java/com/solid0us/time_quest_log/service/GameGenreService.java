package com.solid0us.time_quest_log.service;

import com.solid0us.time_quest_log.model.GameGenres;
import com.solid0us.time_quest_log.model.ServiceResult;
import com.solid0us.time_quest_log.repositories.GameGenreRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.List;

@Service
public class GameGenreService {
    @Autowired
    private GameGenreRepository gameGenreRepository;

    public ServiceResult<List<GameGenres>> getAllGameGenres(){
        return ServiceResult.success(gameGenreRepository.findAll());
    }

    public ServiceResult<List<GameGenres>> createGameGenres(List<GameGenres> gameGenres){
        return ServiceResult.success(gameGenreRepository.saveAll(gameGenres));
    }
}
