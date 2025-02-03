package com.solid0us.time_quest_log.repositories;

import com.solid0us.time_quest_log.model.GameGenreIdComposite;
import com.solid0us.time_quest_log.model.GameGenres;
import org.springframework.data.jpa.repository.JpaRepository;

public interface GameGenreRepository extends JpaRepository<GameGenres, GameGenreIdComposite>{
}
