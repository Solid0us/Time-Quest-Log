package com.solid0us.time_quest_log.service;

import com.solid0us.time_quest_log.model.Genres;
import com.solid0us.time_quest_log.model.IGDBGenre;
import com.solid0us.time_quest_log.repositories.GenreRepository;
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

    public List<Genres> getAllGenres() {
        List<Genres> dbGenres = genreRepository.findAll();
        if (dbGenres.isEmpty()) {
            List<IGDBGenre> igdbGenres = getAllGenresFromIGDB();
            List<Genres> genresToAdd = new ArrayList<>();
            for (IGDBGenre genre : igdbGenres) {
                Genres newGenre = new Genres();
                newGenre.setId(genre.getId());
                newGenre.setName(genre.getName());
                genresToAdd.add(newGenre);
            }
            return genreRepository.saveAll(genresToAdd);
        }
        return dbGenres;
    }

    public List<IGDBGenre> getAllGenresFromIGDB (){
        try {
            return igdbService.getGenres();
        } catch (InterruptedException e) {
            throw new RuntimeException(e);
        }
    }
}
