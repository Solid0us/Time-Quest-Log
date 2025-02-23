package com.solid0us.time_quest_log.repositories;

import com.solid0us.time_quest_log.model.UserGames;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;

import java.util.List;
import java.util.UUID;

public interface UserGameRepository extends JpaRepository<UserGames, UUID> {
    List<UserGames> findByUser_Id(UUID userId);


    @Query(value = """
           SELECT COUNT(DISTINCT game_id)
           FROM user_games
           WHERE user_id = :userId;
    """, nativeQuery = true)
    int findDistinctGamesByUserId(@Param("userId") UUID userId);
}
