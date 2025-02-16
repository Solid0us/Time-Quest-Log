package com.solid0us.time_quest_log.service;

import com.solid0us.time_quest_log.model.ErrorDetail;
import com.solid0us.time_quest_log.model.Genres;
import com.solid0us.time_quest_log.model.IGDBGenre;
import com.solid0us.time_quest_log.model.ServiceResult;
import com.solid0us.time_quest_log.repositories.GenreRepository;
import jakarta.transaction.Transactional;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.List;

@Service
public class GenreService {

    @Autowired
    IGDBService igdbService;

    @Autowired
    GenreRepository genreRepository;

    @Transactional(rollbackOn = Exception.class)
    public ServiceResult<List<Genres>> getAllGenres() {
        List<Genres> dbGenres = genreRepository.findAll();
        List<ErrorDetail> errors = new ArrayList<ErrorDetail>();
        if (dbGenres.isEmpty()) {
            ServiceResult<List<IGDBGenre>> igdbGenres = getAllGenresFromIGDB();
            List<Genres> genresToAdd = new ArrayList<>();
            for (IGDBGenre genre : igdbGenres.getData()) {
                Genres newGenre = new Genres();
                newGenre.setId(genre.getId());
                newGenre.setName(genre.getName());
                genresToAdd.add(newGenre);
            }
            return ServiceResult.success(genreRepository.saveAll(genresToAdd));
        }
        return ServiceResult.success(dbGenres);
    }

    @Transactional(rollbackOn = Exception.class)
    public ServiceResult<List<IGDBGenre>> getAllGenresFromIGDB (){
        try {
            return igdbService.getGenres();
        } catch (InterruptedException e) {
            throw new RuntimeException(e);
        }
    }
}
