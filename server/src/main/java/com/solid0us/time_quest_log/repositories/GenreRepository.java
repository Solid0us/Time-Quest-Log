package com.solid0us.time_quest_log.repositories;

import com.solid0us.time_quest_log.model.Genres;
import org.springframework.data.jpa.repository.JpaRepository;

public interface GenreRepository extends JpaRepository<Genres, Integer>{
}
