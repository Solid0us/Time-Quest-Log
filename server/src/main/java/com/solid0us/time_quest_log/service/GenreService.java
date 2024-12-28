package com.solid0us.time_quest_log.service;

import com.solid0us.time_quest_log.model.IGDBGenre;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.io.IOException;
import java.util.List;

@Service
public class GenreService {

    @Autowired
    IGDBService igdbService;

    public List<IGDBGenre> getAllGenres() {
        try {
            return igdbService.getGenres();
        } catch (IOException | InterruptedException e) {
            throw new RuntimeException(e);
        }
    }
}
