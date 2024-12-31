package com.solid0us.time_quest_log.controller;

import com.solid0us.time_quest_log.model.ApiResponse;
import com.solid0us.time_quest_log.service.GenreService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequestMapping("/api/v1/genres")
public class GenreController {
    @Autowired
    GenreService genreService;

    @RequestMapping({"/", ""})
    public ResponseEntity<ApiResponse<?>> getGenres() {
        return ResponseEntity.ok().body(ApiResponse.success("", genreService.getAllGenres().getData()));
    }
}
