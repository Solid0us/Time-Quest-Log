package com.solid0us.time_quest_log.repositories;

import com.solid0us.time_quest_log.model.Games;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;

public interface GameRepository extends JpaRepository<Games, Integer> {
    @Query("Select g from Games g WHERE LOWER(g.name) = LOWER(:name)")
    Games findByName(String name);
}
