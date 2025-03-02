package com.solid0us.time_quest_log.repositories;

import com.solid0us.time_quest_log.model.DTOs.UserGameWithHoursDTO;
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

    @Query(value = """
          SELECT
              g.id AS gameId,
              g.name AS gameName,
              g.cover_url AS coverUrl,
              ug.exe_name AS exeName,
              STRING_AGG(DISTINCT gen.name, ', ') AS genres,
              COALESCE(SUM(EXTRACT(EPOCH FROM (gs.end_time - gs.start_time)) / 3600)::double precision, 0) AS hoursPlayed
          FROM
              user_games ug
          JOIN
              games g ON g.id = ug.game_id
          JOIN
              users u ON u.id = ug.user_id
          JOIN
              game_genre g_gen ON g_gen.game_id = g.id
          JOIN
              genres gen ON gen.id = g_gen.genre_id
          LEFT JOIN
              game_sessions gs ON gs.game_id = g.id
          WHERE
              u.id = :userId
          GROUP BY
              g.id, g.name, ug.exe_name
          ORDER BY g.name;
    """, nativeQuery = true)
    List<UserGameWithHoursDTO> getUserGameWithHoursPlayed(@Param("userId") UUID userId);
}
