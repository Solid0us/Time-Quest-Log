package com.solid0us.time_quest_log.controller;

import com.solid0us.time_quest_log.model.Genres;
import com.solid0us.time_quest_log.service.GenreService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import java.util.List;

@RestController
@RequestMapping("/api/v1/genres")
public class GenreController {
    @Autowired
    GenreService genreService;

    @RequestMapping({"/", ""})
    public List<Genres> getGenres() {
        return genreService.getAllGenres();
    }
}
